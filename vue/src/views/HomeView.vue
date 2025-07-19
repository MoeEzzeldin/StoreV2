<template>
  <div class="home-view">
    <Slider />
    <section class="container py-4">
      <h3 class="section-title">Under $200</h3>
      <div
        v-if="underTwoHundred.length"
        class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4"
      >
        <div
          v-for="product in underTwoHundred"
          :key="product.productId"
          class="col"
          @click="selectProduct(product.productId)"
        >
          <Product :item="product" />
        </div>
      </div>

      <div v-else class="text-center py-5">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>
    </section>

    <section class="container py-4">
      <h3 class="section-title">Under $500</h3>
      <div
        v-if="underFiveHundred.length"
        class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4"
      >
        <div
          v-for="product in underFiveHundred"
          :key="product.productId"
          class="col"
          @click="selectProduct(product.productId)"
        >
          <Product :item="product" />
        </div>
      </div>

      <div v-else class="text-center py-5">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>
    </section>

    <section class="container py-4">
      <h3 class="section-title">Over $500</h3>
      <div
        v-if="fiveHundredPlus.length"
        class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4"
      >
        <div
          v-for="product in fiveHundredPlus"
          :key="product.productId"
          class="col"
          @click="selectProduct(product.productId)"
        >
          <Product :item="product" />
        </div>
      </div>

      <div v-else class="text-center py-5">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import Product from "../components/Products/Product.vue";
import ProductService from "../services/ProductService";
import Slider from "../components/Slider.vue";
import { ref, onMounted } from "vue";
export default {
  name: "HomeView",
  components: {
    Product,
    Slider,
  },
  data() {
    return {
      underTwoHundred: [],
      underFiveHundred: [],
      fiveHundredPlus: [],
      productId: null,
    };
  },
  methods: {
    //this function makes 3 calls to get products within price ranges
    //and sets the data to the respective arrays
    getProductCards() {
      ProductService.getProductsByPriceRange()
        //assuming that the backend is sending me a dictiomnary with keys as price ranges
        .then((response) => {
          // For products under $200, we shuffle and slice the first 6 products
          this.underTwoHundred =
            response.data["under-200"]
              .map((value) => ({ value, sort: Math.random() }))
              .sort((a, b) => a.sort - b.sort)
              .map(({ value }) => value)
              .slice(0, 6) || [];
          // For products under $500, we also shuffle and slice
          this.underFiveHundred =
            response.data["under-500"]
              .map((value) => ({ value, sort: Math.random() }))
              .sort((a, b) => a.sort - b.sort)
              .map(({ value }) => value)
              .slice(0, 6) || [];
          // For products over $500, we also shuffle and slice
          this.fiveHundredPlus =
            response.data["over-500"]
              .map((value) => ({ value, sort: Math.random() }))
              .sort((a, b) => a.sort - b.sort)
              .map(({ value }) => value)
              .slice(0, 6) || [];
        })
        .catch((error) => {
          console.error("Error fetching product cards:", error);
        });
    },

    selectProduct(productId) {
      this.$router.push({
        name: "product-view",
        params: { id: productId },
      });
    },
  },

  computed: {

  },
  created() {
    this.getProductCards();
  },
};
</script>

<style scoped>
.home-view {
  min-height: 100vh;
}

.col {
  cursor: pointer;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.col:hover {
  transform: translateY(-5px);
}

.section-title {
  font-size: 1.8rem;
  font-weight: 600;
  margin-bottom: var(--spacing-lg);
  padding-bottom: var(--spacing-sm);
  border-bottom: 2px solid var(--color-dark-secondary);
  color: var(--color-accent);
}

.spinner-border {
  width: 3rem;
  height: 3rem;
  color: var(--color-accent) !important;
}
</style>
