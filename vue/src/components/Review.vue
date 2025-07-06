<template>
  <div class="review-component">
    <!-- Reviews List Section -->
    <div class="card review-card mb-4">
      <div class="card-header">
        <h3 class="section-title mb-0">Product Reviews</h3>
      </div>
      
      <!-- Reviews List -->
      <div class="reviews-container">
        <div v-if="reviews.length === 0" class="no-reviews-message">
          <div class="text-center py-4">
            <i class="bi bi-chat-square-text fs-1 mb-2 d-block"></i>
            <p>No reviews yet. Be the first to share your thoughts!</p>
          </div>
        </div>
        
        <div v-else class="reviews-list">
          <div v-for="review in reviews" :key="review.review_id" class="review-item border-bottom">
            <div class="d-flex justify-content-between align-items-start mb-2">
              <div class="reviewer-info flex-grow-1">
                <div class="d-flex align-items-center mb-1">
                  <div class="avatar-initials me-2">
                    {{ getInitials(review.reviewer) }}
                  </div>
                  <h6 class="reviewer-name mb-0 text-light">{{ review.reviewer }}</h6>
                </div>
                <div class="rating-stars mb-1">
                  <span v-for="n in 5" :key="n" class="star me-1" :class="{ 'filled': n <= review.rating }">
                    <i class="bi bi-star-fill" v-if="n <= review.rating"></i>
                    <i class="bi bi-star" v-else></i>
                  </span>
                  <small class="text-muted ms-1">({{ review.rating }}/5)</small>
                </div>
              </div>
              
              <!-- User-friendly datetime display -->
              <div class="review-datetime text-end">
                <div class="badge bg-secondary mb-1">
                  <i class="bi bi-clock me-1"></i>
                  {{ formatRelativeTime(review.date) }}
                </div>
                <div class="small text-muted">
                  {{ formatFullDate(review.date) }}
                </div>
              </div>
            </div>
            
            <div class="review-content">
              <h6 class="review-title text-primary mb-2">{{ review.title }}</h6>
              <p class="review-comment text-light mb-0">{{ review.comment }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Add Review Form Section -->
    <div class="card review-form-card">
      <div class="card-header">
        <h3 class="section-title mb-0">Write a Review</h3>
      </div>
      <div class="card-body">
        <form @submit.prevent="addReview" class="row g-3">
          <div class="col-12">
            <label for="review-title" class="form-label">Review Title</label>
            <input 
              id="review-title" 
              type="text" 
              v-model="newReview.title" 
              class="form-control" 
              placeholder="Summarize your experience..." 
              required 
            />
          </div>
          
          <div class="col-12">
            <label for="review-comment" class="form-label">Your Review</label>
            <textarea 
              id="review-comment" 
              v-model="newReview.comment" 
              class="form-control" 
              placeholder="Share your thoughts about this product..." 
              rows="3" 
              required
            ></textarea>
          </div>
          
          <div class="col-md-6">
            <label for="starRating" class="form-label">Rating</label>
            <div class="rating-select">
              <select 
                id="starRating" 
                v-model="newReview.rating" 
                class="form-select" 
                required
              >
                <option disabled value="">Select rating</option>
                <option v-for="n in 5" :key="n" :value="n">{{ n }} star{{ n>1?'s':'' }}</option>
              </select>
            </div>
          </div>
          
          <div class="col-md-6 d-flex align-items-end">
            <button type="submit" class="btn submit-review-btn w-100">
              <i class="bi bi-send me-2"></i> Post Review
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import ReviewService from '../services/ReviewService';
import { mapState } from 'vuex';
export default {
  name: 'Reviews',
  props: {
    productId: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      reviews: [],
      newReview: this.getEmptyReviewObject(),
    };
  },
  methods: {
    getInitials(name) {
      return name
        .split(' ')
        .map(word => word.charAt(0).toUpperCase())
        .join('')
        .substring(0, 2);
    },
    
    formatRelativeTime(dateString) {
      if (!dateString) return 'Unknown';
      
      const date = new Date(dateString);
      const now = new Date();
      const diffInSeconds = Math.floor((now - date) / 1000);
      
      if (diffInSeconds < 60) return 'Just now';
      if (diffInSeconds < 3600) return `${Math.floor(diffInSeconds / 60)}m ago`;
      if (diffInSeconds < 86400) return `${Math.floor(diffInSeconds / 3600)}h ago`;
      if (diffInSeconds < 604800) return `${Math.floor(diffInSeconds / 86400)}d ago`;
      if (diffInSeconds < 2419200) return `${Math.floor(diffInSeconds / 604800)}w ago`;
      
      return `${Math.floor(diffInSeconds / 2419200)}mo ago`;
    },
    
    formatFullDate(dateString) {
      if (!dateString) return '';
      
      const date = new Date(dateString);
      return new Intl.DateTimeFormat('en-US', {
        weekday: 'short',
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      }).format(date);
    },
    
    getEmptyReviewObject() {
      return {
        productId: this.productId,
        reviewer: this.$store.state.user?.username || 'Guest',
        date: new Date(),
        title: '',
        comment: '',
        rating: '',
      };
    },
    fetchReviews() {
      ReviewService.productReviewList(this.productId)
        .then((response) => {
          this.reviews = response.data;
          console.log('Fetched Reviews', this.reviews);
        })
        .catch((error) => {
          console.error(error);
        });
    },

    addReview() {
      // Don't submit if required fields are empty
      if (!this.newReview.title.trim() || !this.newReview.rating) {
        return;
      }
      // Make sure productId is set correctly
      this.newReview.productId = this.productId;
      
      ReviewService.addAReview(this.newReview)
        .then(() => {
          this.fetchReviews();
          // Reset the entire form using our helper method
          this.newReview = this.getEmptyReviewObject();
        })
        .catch((error) => {
          console.error(error);
        });
    },
  },
  created() {
    this.fetchReviews();
  },
};
</script>

<style scoped>
.review-component {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.card {
  background: linear-gradient(145deg, #2b2f33, #24282c);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: var(--radius-lg);
  overflow: hidden;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.2);
}

.card-header {
  background-color: rgba(0, 0, 0, 0.2);
  border-bottom: 1px solid rgba(255, 255, 255, 0.08);
  padding: 1rem 1.5rem;
}

.section-title {
  color: #fff;
  font-size: 1.3rem;
  font-weight: 700;
  letter-spacing: 0.5px;
}

.section-title i {
  color: var(--color-accent);
}

.reviews-container {
  max-height: 450px;
  overflow-y: auto;
  scrollbar-width: thin;
  scrollbar-color: var(--color-accent) transparent;
}

.reviews-container::-webkit-scrollbar {
  width: 8px;
}

.reviews-container::-webkit-scrollbar-track {
  background: transparent;
}

.reviews-container::-webkit-scrollbar-thumb {
  background-color: var(--color-accent);
  border-radius: 8px;
  border: 2px solid #2a2d31;
}

.reviews-list {
  padding: 0;
}

.review-item {
  padding: 1.25rem 1.5rem;
  transition: background-color 0.3s ease;
  border-bottom: 1px solid rgba(255, 255, 255, 0.08) !important;
}

.review-item:hover {
  background-color: rgba(var(--bs-primary-rgb), 0.05);
}

.review-item:last-child {
  border-bottom: none !important;
}

.avatar-initials {
  width: 40px;
  height: 40px;
  background: linear-gradient(45deg, var(--color-accent), #00c4d3);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #fff;
  font-weight: 700;
  font-size: 1rem;
  flex-shrink: 0;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
}

.reviewer-name {
  color: #e9ecef;
  font-size: 1rem;
  font-weight: 600;
}

.review-datetime {
  min-width: 140px;
}

.review-datetime .small.text-muted {
  color: #9a9fa5 !important; /* Lighter muted color for visibility */
  font-size: 0.75rem;
}

.rating-stars .star i {
  color: var(--color-text-muted);
  transition: color 0.2s ease;
}

.rating-stars .star.filled i {
  color: #ffc107; /* Gold color for filled stars */
}

.review-title {
  color: var(--color-accent) !important;
  font-size: 1.1rem;
  font-weight: 600;
}

.review-comment {
  color: #ced4da;
  line-height: 1.6;
  font-size: 0.95rem;
}

.no-reviews-message {
  color: var(--color-text-muted);
  font-size: 0.95rem;
}

.no-reviews-message i {
  color: var(--color-accent);
  opacity: 0.8;
}

/* Form styling */
.card-body {
  padding: 1.5rem;
  background-color: rgba(0, 0, 0, 0.1);
}

.form-label {
  color: #e9ecef;
  font-size: 0.9rem;
  font-weight: 600;
  margin-bottom: 0.5rem;
}

.form-control,
.form-select {
  background-color: #2c3034;
  border: 1px solid rgba(255, 255, 255, 0.1);
  color: #fff;
  font-size: 0.95rem;
  padding: 0.75rem 1rem;
  border-radius: var(--radius-md);
  transition: all 0.3s ease;
}

.form-control:focus,
.form-select:focus {
  background-color: #313539;
  border-color: var(--color-accent);
  box-shadow: 0 0 0 3px rgba(var(--bs-primary-rgb), 0.15);
  color: #fff;
}

.form-control::placeholder {
  color: #8a929a;
}

textarea.form-control {
  resize: vertical;
  min-height: 110px;
}

.submit-review-btn {
  background: linear-gradient(45deg, var(--color-accent), #00c4d3);
  color: #fff;
  font-weight: 700;
  padding: 0.85rem 1.25rem;
  border: none;
  border-radius: var(--radius-md);
  transition: all 0.3s ease;
  box-shadow: 0 4px 15px rgba(0, 173, 181, 0.2);
}

.submit-review-btn:hover {
  transform: translateY(-3px);
  box-shadow: 0 6px 20px rgba(0, 173, 181, 0.3);
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .review-item .d-flex {
    flex-direction: column;
  }
  
  .review-datetime {
    margin-top: 0.75rem;
    text-align: left !important;
    min-width: auto;
  }
  
  .review-datetime .badge {
    display: inline-block;
  }
  
  .avatar-initials {
    width: 36px;
    height: 36px;
    font-size: 0.8rem;
  }
}
</style>
