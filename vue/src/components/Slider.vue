<template>
  <div id="marketCarousel" class="carousel slide mb-5" data-bs-ride="carousel">
    <!-- Carousel indicators -->
    <div class="carousel-indicators">
      <button type="button" data-bs-target="#marketCarousel" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
      <button type="button" data-bs-target="#marketCarousel" data-bs-slide-to="1" aria-label="Slide 2"></button>
      <button type="button" data-bs-target="#marketCarousel" data-bs-slide-to="2" aria-label="Slide 3"></button>
    </div>
    
    <!-- Carousel items -->
    <div class="carousel-inner">
      <div class="carousel-item active">
        <div class="carousel-content bg-secondary text-center py-5">
          <div class="container py-5">
            <h1 class="display-4 text-primary fw-bold">Welcome to Our Store</h1>
            <p class="lead text-light-emphasis mb-4">Discover amazing products at unbeatable prices</p>
            <button class="btn btn-primary btn-lg">Shop Now</button>
          </div>
        </div>
      </div>
      
      <div class="carousel-item">
        <div class="carousel-content bg-secondary text-center py-5">
          <div class="container py-5">
            <h1 class="display-4 text-primary fw-bold">Summer Sale</h1>
            <p class="lead text-light-emphasis mb-4">Up to 50% off on selected items</p>
            <button class="btn btn-primary btn-lg">View Deals</button>
          </div>
        </div>
      </div>
      
      <div class="carousel-item">
        <div class="carousel-content bg-secondary text-center py-5">
          <div class="container py-5">
            <h1 class="display-4 text-primary fw-bold">New Arrivals</h1>
            <p class="lead text-light-emphasis mb-4">Check out our latest products</p>
            <button class="btn btn-primary btn-lg">Explore</button>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Carousel controls -->
    <button class="carousel-control-prev" type="button" data-bs-target="#marketCarousel" data-bs-slide="prev">
      <span class="carousel-control-prev-icon" aria-hidden="true"></span>
      <span class="visually-hidden">Previous</span>
    </button>
    <button class="carousel-control-next" type="button" data-bs-target="#marketCarousel" data-bs-slide="next">
      <span class="carousel-control-next-icon" aria-hidden="true"></span>
      <span class="visually-hidden">Next</span>
    </button>
  </div>
</template>

<script>
export default {
  data() {
    return {};
  },
  mounted() {
    // Bootstrap carousel initialization happens automatically
  }
};
</script>

<style scoped>
.carousel-content {
  min-height: 60vh;
  border-radius: 0.5rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  display: flex;
  align-items: center;
  justify-content: center;
}

.text-primary {
  color: var(--color-accent, #00ADB5) !important;
}

.text-light-emphasis {
  color: rgba(255, 255, 255, 0.8) !important;
}

.carousel-control-prev,
.carousel-control-next {
  width: 5%;
}

.btn-primary {
  background-color: var(--color-accent, #00ADB5);
  border-color: var(--color-accent, #00ADB5);
}

.btn-primary:hover {
  background-color: var(--color-accent-hover, #008e96);
  border-color: var(--color-accent-hover, #008e96);
}
</style>
