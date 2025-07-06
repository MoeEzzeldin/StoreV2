<template>
  <div class="home-view">
    <Slider />
    <section class="container py-4">
      <h3 class="section-title">Under $200</h3>
      <div v-if="underTwoHundred.length" class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
        <div 
          v-for="product in randomProductsOne" 
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
      <div v-if="underFiveHundred.length" class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
        <div 
          v-for="product in randomProductsTwo" 
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
      <div v-if="fiveHundredPlus.length" class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
        <div 
          v-for="product in randomProductsThree" 
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
import Product from '../components/Products/Product.vue';
import ProductService from '../services/ProductService';
import Slider from '../components/Slider.vue';
export default {
  name: 'HomeView',
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
    getProducts() {
      ProductService.productFilterOne()
        .then((response) => {
          this.underTwoHundred = response.data;
        })
        .catch((error) => {
          console.log(error);
        });

      ProductService.productFilterTwo()
        .then((response) => {
          this.underFiveHundred = response.data;
        })
        .catch((error) => {
          console.log(error);
        });

      ProductService.productFilterThree()
        .then((response) => {
          this.fiveHundredPlus = response.data;
        })
        .catch((error) => {
          console.log(error);
        });
    },

    selectProduct(productId) {
      this.$router.push({
        name: 'product-view',
        params: { id: productId },
      });
    },
  },

  computed: {
    randomProductsOne() {
      // Shuffle the array and slice the first 4 elements
      return this.underTwoHundred
        .map((value) => ({ value, sort: Math.random() }))
        .sort((a, b) => a.sort - b.sort)
        .map(({ value }) => value)
        .slice(0, 6);
    },
    randomProductsTwo() {
      return this.underFiveHundred
        .map((value) => ({ value, sort: Math.random() }))
        .sort((a, b) => a.sort - b.sort)
        .map(({ value }) => value)
        .slice(0, 6);
    },
    randomProductsThree() {
      return this.fiveHundredPlus
        .map((value) => ({ value, sort: Math.random() }))
        .sort((a, b) => a.sort - b.sort)
        .map(({ value }) => value)
        .slice(0, 6);
    },
  },
  created() {
    this.getProducts();
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
