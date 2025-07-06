<template>
  <div class="product-search-view">
    <div class="container">
      <h1 class="section-title">Search Results for "{{ productName }}"</h1>
      <div v-if="isLoading" class="d-flex justify-content-center mt-5">
        <div class="spinner-border" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>
      <div v-else-if="searchProduct.length > 0" class="row">
        <div
          class="col-md-4 col-lg-3 mb-4"
          v-for="product in searchProduct"
          :key="product.productId"
        >
          <Product
            :item="product"
            @click="selectProduct(product.productId)"
          />
        </div>
      </div>
      <div v-else class="empty-state mt-5">
        <h2>No products found for "{{ productName }}"</h2>
        <p>Try searching for something else.</p>
      </div>
    </div>
  </div>
</template>

<script>
import Product from '../components/Products/Product.vue';
import ProductService from '../services/ProductService';

export default {
  name: 'ProductSearchView',
  components: {
    Product,
  },
  data() {
    return {
      searchProduct: [],
      isLoading: true,
      productName: this.$route.params.ProductName,
    };
  },
  methods: {
    getProducts(name) {
      this.isLoading = true;
      document.title = `Search for \"${name}\"`;
      ProductService.search(name)
        .then((response) => {
          this.searchProduct = response.data;
        })
        .catch((error) => {
          console.error('Error fetching search results:', error);
        })
        .finally(() => {
          this.isLoading = false;
        });
    },

    selectProduct(productId) {
      this.$router.push({
        name: 'product-view',
        params: { id: productId },
      });
    },
  },
  created() {
    this.getProducts(this.productName);
  },
  watch: {
    '$route.params.ProductName'(newProductName) {
      this.productName = newProductName;
      this.getProducts(newProductName);
    },
  },
};
</script>

<style scoped>
.product-search-view {
  padding: var(--spacing-xl) 0;
  min-height: 80vh;
}

.container {
  max-width: var(--content-width);
  margin: 0 auto;
  padding: 0 var(--spacing-md);
}

.section-title {
  font-size: 2rem;
  font-weight: bold;
  color: var(--color-text-primary);
  margin-bottom: var(--spacing-lg);
  text-align: center;
}

.spinner-border {
  width: 3rem;
  height: 3rem;
  color: var(--color-accent);
}

.empty-state {
  text-align: center;
  padding: var(--spacing-xl);
  background-color: var(--color-dark-secondary);
  border-radius: var(--radius-lg);
  color: var(--color-text-muted);
}

.empty-state h2 {
  font-size: 1.5rem;
  font-weight: bold;
}

.empty-state p {
  font-size: 1.1rem;
}
</style>
