<template>
  <div id="webStore" class="d-flex flex-column min-vh-100 bg-dark text-light">
    <Header :logoUrl="logoUrl" :menuItems="menuItems" />
    <main class="container flex-grow-1 py-5 mt-5">
      <router-view />
    </main>
    <Footer />
  </div>
</template>

<script>
import Header from './components/Header.vue';
import Footer from './components/Footer.vue';
import './assets/main.css';

export default {
  components: {
    Header,
    Footer,
  },
  data() {
    return {
      logoUrl: '../src/assets/1.png',
      width: 0,
      menuItems: [
        { text: 'Home', link: '/home', icon: 'bi bi-house-fill' },
        { text: 'About', link: '#footer', icon: 'bi bi-info-circle-fill' },
        { text: 'Contact', link: '/contact', icon: 'bi bi-person-lines-fill' },
      ],
    };
  },
  methods: {},
  computed: {
  },
};
</script>

<style>
/* App-specific styles */
.img {
  display: block;
  max-width: 100%;
}
</style>
