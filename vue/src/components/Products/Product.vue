<template>
  <div class="card h-100 bg-secondary text-light shadow product-card">
    <div class="img-container position-relative overflow-hidden">
      <img :src="productImage" alt="Product image of {{ item.name }}" class="card-img-top" />
    </div>

    <div class="card-body d-flex flex-column">
      <h5 class="card-title">
        <span class="brand">{{ item.brand }}</span> | {{ item.name }}
      </h5>
      <p class="card-subtitle text-uppercase small text-light-emphasis mb-2">{{ item.type }}</p>
      
      <div class="mt-auto">
        <div class="mb-2">
          <small class="text-light-emphasis d-block">Price</small>
          <span class="fs-4 fw-bold">${{ item.price }}</span>
        </div>
        
        <button @click="AddToCart" type="button" class="btn add-to-cart-btn w-100">
          <i class="bi bi-plus-circle me-2"></i> Add to Cart
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import ImageService from '../../services/ImageService';

export default {
  name: 'product',
  props: ['item'],

  data() {
    return {
    };
  },
  computed: {
    productImage() {
      return ImageService.generateProductImage(this.item);
    }
  },
  methods: {
    AddToCart() {
      alert('Added to cart');
    },
  },

  created() {
  },
};
</script>

<style scoped>
.product-card {
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.product-card:hover {
  /* transform: translateY(-5px); */
  box-shadow: 0 10px 20px rgba(0, 0, 0, 0.3) !important;
}

.img-container {
  height: 180px;
  background-color: rgba(0, 0, 0, 0.2);
}

.card-img-top {
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
  background-color: rgba(255, 255, 255, 0.05); /* Light background for the image */
  backdrop-filter: blur(5px); /* Slight blur effect on the background */
}

.img-container::after {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: linear-gradient(rgba(0, 0, 0, 0), rgba(0, 0, 0, 0.2));
  pointer-events: none;
}

.product-card:hover .card-img-top {
  transform: scale(1.05);
}

.card-title {
  color: var(--color-text-light);
}

.card-title .brand {
  color: var(--color-accent);
  font-weight: 600;
}

.add-to-cart-btn {
  background-color: var(--color-accent);
  color: var(--color-dark);
  font-weight: 600;
}

.add-to-cart-btn:hover {
  background-color: var(--color-accent-dark);
}

.text-light-emphasis {
  color: rgba(255, 255, 255, 0.7) !important;
}

/* Accessibility improvements */
@media (prefers-reduced-motion: reduce) {
  .product-card:hover {
    transform: none;
  }
  
  .product-card:hover .card-img-top {
    transform: none;
  }
}
</style>
