using Adotzee_Backend.DTOs.Review;
using Adotzee_Backend.Models;
using Adotzee_Backend.Models.Enums;
using Adotzee_Backend.Repository.ReviewRepos;

namespace Adotzee_Backend.Services.ReviewServices
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<PagedResponse<ReviewResponseDto>> GetAllAsync(ReviewQueryParametersDto queryParameters)
        {
            var pagedReviews = await _reviewRepository.GetAllAsync(queryParameters);
            
            var dtoList = pagedReviews.Items.Select(MapToDto).ToList();
            
            return new PagedResponse<ReviewResponseDto>(dtoList, pagedReviews.TotalCount, pagedReviews.PageNumber, pagedReviews.PageSize);
        }

        public async Task<List<ReviewResponseDto>> GetFeaturedAsync()
        {
            var reviews = await _reviewRepository.GetFeaturedAsync();
            return reviews.Select(MapToDto).ToList();
        }

        public async Task<ReviewResponseDto?> GetByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null) return null;
            return MapToDto(review);
        }

        public async Task<ReviewResponseDto> CreateAsync(CreateReviewDto createDto, string? ipAddress, string? userAgent)
        {
            var review = new Review
            {
                FullName = createDto.FullName,
                Email = createDto.Email,
                MobileNumber = createDto.MobileNumber,
                City = createDto.City,
                State = createDto.State,
                Course = createDto.Course,
                CollegeName = createDto.CollegeName,
                Rating = createDto.Rating,
                ReviewTitle = createDto.ReviewTitle,
                ReviewMessage = createDto.ReviewMessage,
                StudentPhoto = createDto.StudentPhoto,
                IsAnonymous = createDto.IsAnonymous,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Status = ReviewStatus.Pending,
                VerificationType = VerificationType.None,
                Featured = false
            };

            var created = await _reviewRepository.CreateAsync(review);
            return MapToDto(created);
        }

        public async Task<ReviewResponseDto?> UpdateStatusAsync(int id, UpdateReviewDto updateDto, string approvedBy)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null) return null;

            if (updateDto.Status.HasValue)
            {
                review.Status = updateDto.Status.Value;
                if (updateDto.Status.Value == ReviewStatus.Approved)
                {
                    review.ApprovedAt = DateTime.UtcNow;
                    review.ApprovedBy = approvedBy;
                }
            }

            if (updateDto.Featured.HasValue) review.Featured = updateDto.Featured.Value;
            if (updateDto.DisplayName != null) review.DisplayName = updateDto.DisplayName;
            if (updateDto.DisplayInitials != null) review.DisplayInitials = updateDto.DisplayInitials;
            if (updateDto.VerificationType.HasValue) review.VerificationType = updateDto.VerificationType.Value;

            var updated = await _reviewRepository.UpdateAsync(review);
            return MapToDto(updated);
        }

        public async Task<ReviewResponseDto?> ToggleFeatureAsync(int id, bool featured)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null) return null;

            review.Featured = featured;
            var updated = await _reviewRepository.UpdateAsync(review);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null) return false;

            await _reviewRepository.DeleteAsync(review);
            return true;
        }

        private ReviewResponseDto MapToDto(Review review)
        {
            var displayName = review.DisplayName ?? (review.IsAnonymous ? "Anonymous User" : review.FullName);
            var displayInitials = review.DisplayInitials ?? (review.IsAnonymous ? "AU" : GetInitials(review.FullName));
            
            return new ReviewResponseDto
            {
                Id = review.Id,
                FullName = displayName, // For frontend display
                Email = review.Email, // Might want to hide this for public later or separate Admin/Public DTOs, but for now we send it all and let frontend decide or we can obscure it. Wait, for public we shouldn't send Email.
                MobileNumber = review.MobileNumber,
                City = review.City,
                State = review.State,
                Course = review.Course,
                CollegeName = review.CollegeName,
                Rating = review.Rating,
                ReviewTitle = review.ReviewTitle,
                ReviewMessage = review.ReviewMessage,
                StudentPhoto = review.StudentPhoto,
                VerificationType = review.VerificationType.ToString(),
                Status = review.Status.ToString(),
                Featured = review.Featured,
                DisplayName = review.DisplayName,
                DisplayInitials = review.DisplayInitials,
                IsAnonymous = review.IsAnonymous,
                CreatedAt = review.CreatedAt,
                ApprovedAt = review.ApprovedAt
            };
        }

        private string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return "U";
            var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0][0].ToString().ToUpper();
            return $"{parts[0][0]}{parts[^1][0]}".ToUpper();
        }
    }
}
