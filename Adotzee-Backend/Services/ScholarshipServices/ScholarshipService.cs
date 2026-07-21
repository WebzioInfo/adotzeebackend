using Adotzee_Backend.Data;
using Adotzee_Backend.DTOs.ScholarshipDTOs;
using Adotzee_Backend.Models;
using Adotzee_Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adotzee_Backend.Services.ScholarshipServices
{
    public class ScholarshipService : IScholarshipService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<ScholarshipService> _logger;

        public ScholarshipService(AppDbContext context, IEmailService emailService, ILogger<ScholarshipService> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<ApiResponse<IEnumerable<ScholarshipDTO>>> GetActiveScholarshipsAsync()
        {
            var scholarships = await _context.Scholarships
                .Where(s => s.IsActive)
                .Select(s => new ScholarshipDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Provider = s.Provider,
                    Type = s.Type,
                    Status = s.Status,
                    Amount = s.Amount,
                    Description = s.Description,
                    EligibilityJson = s.EligibilityJson,
                    Disclaimer = s.Disclaimer,
                    ApplicationStartDate = s.ApplicationStartDate,
                    ApplicationEndDate = s.ApplicationEndDate,
                    BannerImageUrl = s.BannerImageUrl,
                    TermsAndConditions = s.TermsAndConditions
                })
                .ToListAsync();

            return ApiResponse<IEnumerable<ScholarshipDTO>>.SuccessResponse(scholarships, "Scholarships retrieved successfully.");
        }

        public async Task<ApiResponse<string>> ApplyForScholarshipAsync(CreateScholarshipEnquiryDTO applicationDto)
        {
            var scholarship = await _context.Scholarships.FindAsync(applicationDto.ScholarshipId);
            if (scholarship == null || !scholarship.IsActive)
            {
                return ApiResponse<string>.FailResponse("Scholarship not found or not active.");
            }

            // Check if already applied
            var existingEnquiry = await _context.ScholarshipEnquiries
                .FirstOrDefaultAsync(e => e.ScholarshipId == applicationDto.ScholarshipId && e.EmailAddress == applicationDto.EmailAddress);

            if (existingEnquiry != null)
            {
                return ApiResponse<string>.FailResponse("You have already applied for this scholarship.");
            }

            var enquiry = new ScholarshipEnquiry
            {
                ScholarshipId = applicationDto.ScholarshipId,
                FullName = applicationDto.FullName,
                MobileNumber = applicationDto.MobileNumber,
                EmailAddress = applicationDto.EmailAddress,
                State = applicationDto.State,
                PlusTwoPercentage = applicationDto.PlusTwoPercentage,
                PreferredCourse = applicationDto.PreferredCourse,
                PreferredCollege = applicationDto.PreferredCollege
            };

            _context.ScholarshipEnquiries.Add(enquiry);
            await _context.SaveChangesAsync();

            // Fire and forget emails
            _ = SendEmailsAsync(enquiry, scholarship);

            return ApiResponse<string>.SuccessResponse("Application submitted successfully.", "Application submitted successfully.");
        }

        private async Task SendEmailsAsync(ScholarshipEnquiry enquiry, Scholarship scholarship)
        {
            try
            {
                // Send email to student
                string studentSubject = $"Application Received: {scholarship.Name}";
                string studentBody = $@"
                    <h2>Dear {enquiry.FullName},</h2>
                    <p>Thank you for expressing interest in the <strong>{scholarship.Name}</strong> provided by <strong>{scholarship.Provider}</strong>.</p>
                    <p>This email confirms that we have received your application successfully. Our admissions team will review your profile and contact you soon.</p>
                    <br>
                    <p>Best Regards,</p>
                    <p>Adotzee Team</p>
                    <p><small>{scholarship.Disclaimer}</small></p>
                ";
                await _emailService.SendEmailAsync(enquiry.EmailAddress, studentSubject, studentBody);

                // Send email to admin
                string adminEmail = "admissions@adotzee.in"; // Can be moved to configuration
                string adminSubject = $"New Scholarship Application - {enquiry.FullName}";
                string adminBody = $@"
                    <h2>New Scholarship Application Received</h2>
                    <p><strong>Scholarship:</strong> {scholarship.Name}</p>
                    <p><strong>Applicant Name:</strong> {enquiry.FullName}</p>
                    <p><strong>Mobile:</strong> {enquiry.MobileNumber}</p>
                    <p><strong>Email:</strong> {enquiry.EmailAddress}</p>
                    <p><strong>State:</strong> {enquiry.State}</p>
                    <p><strong>+2 Percentage:</strong> {enquiry.PlusTwoPercentage}</p>
                    <p><strong>Course:</strong> {enquiry.PreferredCourse}</p>
                    <p><strong>College:</strong> {enquiry.PreferredCollege}</p>
                ";
                await _emailService.SendEmailAsync(adminEmail, adminSubject, adminBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send scholarship application emails.");
            }
        }
    }
}
