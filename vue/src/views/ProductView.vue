<template>
  <div class="product-view">
    <div class="container py-4">
      <!-- Breadcrumb Navigation -->
      <nav aria-label="breadcrumb" class="mb-4">
        <ol class="breadcrumb">
          <li class="breadcrumb-item"><router-link to="/home">Home</router-link></li>
          <li class="breadcrumb-item active" aria-current="page">Product Details</li>
        </ol>
      </nav>

      <!-- Product Header Section -->
      <header class="product-header mb-4">
        <div class="row align-items-center">
          <div class="col-md-8">
            <h1 class="product-title">{{ product.name }}</h1>
            <p class="product-brand">{{ product.brand }}</p>
          </div>
          <div class="col-md-4">
            <div class="rating-card">
              <div class="rating-value">{{ avgRating || 'No' }}</div>
              <div class="rating-label">{{ avgRating ? 'Rating' : 'Ratings Yet' }}</div>
              <div v-if="avgRating" class="rating-stars">
                <span v-for="n in 5" :key="n" class="star" :class="{ 'filled': n <= Math.round(avgRating) }">★</span>
              </div>
            </div>
          </div>
        </div>
      </header>

      <!-- Main Content -->
      <div class="row g-4">
        <!-- Product Card Column -->
        <div class="col-lg-5">
          <div class="product-image-section">
            <Product :key="productId" :item="product" />
          </div>
        </div>
        
        <!-- Reviews Column -->
        <div class="col-lg-7">
          <div class="product-details-section">
            <Review :productId="productId" />
          </div>
        </div>
      </div>

      <!-- Recommended Products Section (placeholder) -->
      <section class="recommended-products mt-5">
        <h2 class="section-title">You Might Also Like</h2>
        <div class="row">
          <!-- This section would be populated with recommended products -->
          <div class="col-12 text-center text-muted py-4">
            <i class="bi bi-arrow-repeat"></i> Loading recommendations...
          </div>
        </div>
      </section>
    </div>
  </div>
</template>

<script>
import ProductService from '../services/ProductService';
import ReviewService from '../services/ReviewService';
import Product from '../components/Products/Product.vue';
import Review from '../components/Review.vue';

export default {
  name: 'ProductView',
  components: { Product, Review },
  data() {
    return {
      productId: Number(this.$route.params.id),
      product: [],
      avgRating: 0,
    };
  },
  methods: {
    getProductById() {
      ProductService.getProduct(this.productId)
        .then((response) => {
          this.product = response.data;
          document.title = `${this.product.name} | Web Store`;
        })
        .catch((error) => {
          console.error('Error fetching product:', error);
        });
    },
    getAvgRating() {
      ReviewService.avg(this.productId)
        .then((response) => {
          this.avgRating = response.data;
        })
        .catch((error) => {
          console.error('Error fetching average rating:', error);
        });
    },
    scrollToTop() {
      window.scrollTo({
        top: 0,
        behavior: 'smooth',
      });
    },
  },
  created() {
    this.getProductById();
    this.getAvgRating();
    this.scrollToTop();
  },
};
</script>

<style scoped>
.product-view {
  padding-bottom: 3rem;
}

.breadcrumb {
  background-color: transparent;
  padding: 0.5rem 0;
}

.breadcrumb-item a {
  color: var(--color-accent);
  text-decoration: none;
}

.breadcrumb-item.active {
  color: var(--color-text-muted);
}

.product-header {
  border-bottom: 1px solid var(--color-dark-secondary);
  padding-bottom: 1.5rem;
}

.product-title {
  color: var(--color-text-light);
  font-size: 2rem;
  font-weight: 600;
  margin-bottom: 0.25rem;
}

.product-brand {
  color: var(--color-accent);
  font-size: 1.1rem;
  margin-bottom: 0;
}

.rating-card {
  background-color: var(--color-dark-secondary);
  border-radius: var(--radius-lg);
  padding: 1rem;
  text-align: center;
  box-shadow: var(--shadow-sm);
  border: 1px solid rgba(255, 255, 255, 0.05);
}

.rating-value {
  font-size: 2rem;
  font-weight: 700;
  color: var(--color-accent);
  line-height: 1;
}

.rating-label {
  color: var(--color-text-muted);
  font-size: 0.85rem;
  margin-bottom: 0.5rem;
}

.rating-stars {
  display: flex;
  justify-content: center;
  gap: 4px;
}

.star {
  color: var(--color-text-muted);
  font-size: 1.2rem;
}

.star.filled {
  color: var(--color-accent);
}

.section-title {
  color: var(--color-accent);
  font-size: 1.5rem;
  margin-bottom: 1.5rem;
  position: relative;
  padding-bottom: 0.75rem;
}

.section-title::after {
  content: "";
  position: absolute;
  bottom: 0;
  left: 0;
  width: 60px;
  height: 3px;
  background-color: var(--color-accent);
}

/* Responsive adjustments */
@media (max-width: 992px) {
  .product-image-section {
    margin-bottom: 2rem;
  }
  
  .rating-card {
    margin-top: 1rem;
  }
}

@media (max-width: 768px) {
  .product-title {
    font-size: 1.75rem;
  }
}
</style>
