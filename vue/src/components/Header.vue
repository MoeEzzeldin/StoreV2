<template>
  <header class="navbar navbar-expand-lg navbar-dark bg-dark fixed-top">
    <div class="container py-2">
      <!-- Logo and Title -->
      <router-link to="/home" class="navbar-brand d-flex align-items-center">
        <div class="position-relative me-2">
          <img :src="logoUrl" alt="Web Store Logo" class="logo-img" />
          <div class="logo-glow"></div>
        </div>
        <h1 class="brand-title mb-0">Market</h1>
      </router-link>

      <!-- Hamburger Menu for Mobile -->
      <button class="navbar-toggler border-0" type="button" data-bs-toggle="collapse" data-bs-target="#navbarContent"
        aria-controls="navbarContent" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>

      <!-- Collapsible Content -->
      <div class="collapse navbar-collapse" id="navbarContent">
        <!-- Search Bar -->
        <form class="nav-search mx-lg-auto my-3 my-lg-0" @submit.prevent="onSearch()">
          <div class="input-group">
            <input 
              class="form-control search-input"
              type="search"
              v-model="searchQuery"
              placeholder="Search products..."
              aria-label="Search products"
            />
            <button class="btn btn-primary search-btn" type="submit">
              <i class="bi bi-search"></i>
              <span class="d-none d-sm-inline ms-1">Search</span>
            </button>
          </div>
        </form>

        <!-- Navigation Links & User Menu -->
        <div class="navbar-nav ms-auto align-items-center">
          <!-- Cart Button (always visible) -->
          <a href="#" class="nav-link position-relative me-3">
            <i class="bi bi-cart3 fs-5"></i>
            <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-primary">
              0
            </span>
          </a>

          <!-- Guest Menu -->
          <div v-if="!isLoggedIn" class="d-flex gap-2">
            <router-link :to="{ name: 'login' }" class="nav-item">
              <button @click="scrollToTopAndNavigate()" class="btn btn-outline-primary btn-sm">
                <i class="bi bi-person me-1"></i>Login
              </button>
            </router-link>
            <router-link :to="{ name: 'register' }" class="nav-item">
              <button @click="scrollToTopAndNavigate()" class="btn btn-primary btn-sm">
                <i class="bi bi-person-plus me-1"></i>Register
              </button>
            </router-link>
          </div>

          <!-- User Menu (Dropdown) -->
          <div v-else class="nav-item dropdown">
            <a class="nav-link dropdown-toggle d-flex align-items-center" href="#" role="button" 
               data-bs-toggle="dropdown" aria-expanded="false">
              <div class="avatar-circle me-2">
                <i class="bi bi-person-fill"></i>
              </div>
              <span class="d-none d-sm-inline">{{ $store.state.user.username || 'User' }}</span>
            </a>
            <ul class="dropdown-menu dropdown-menu-end">
              <li v-for="item in menuItems" :key="item.text">
                <a :href="item.link" class="dropdown-item">
                  <i :class="item.icon" class="me-2"></i>
                  {{ item.text }}
                </a>
              </li>
              <li><hr class="dropdown-divider"></li>
              <li>
                <router-link :to="{ name: 'logout' }" class="dropdown-item text-danger" @click="scrollToTopAndNavigate()">
                  <i class="bi bi-box-arrow-right me-2"></i>Logout
                </router-link>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </header>
</template>

<script>
import { Dropdown } from 'bootstrap';

export default {
  name: 'Header',
  props: {
    logoUrl: {
      type: String,
      required: true,
    },
    menuItems: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      searchQuery: '',
      dropdowns: [],
    };
  },
  computed: {
    isLoggedIn() {
      return !!this.$store.state.token;
    },
  },
  mounted() {
    // Initialize all dropdowns
    const dropdownElements = this.$el.querySelectorAll('.dropdown-toggle');
    this.dropdowns = Array.from(dropdownElements).map(element => {
      return new Dropdown(element);
    });
  },
  beforeUnmount() {
    // Clean up dropdowns when component is destroyed
    this.dropdowns.forEach(dropdown => {
      dropdown.dispose();
    });
  },
  methods: {
    onSearch() {
      if (!this.searchQuery.trim()) return;
      
      this.$router.push({
        name: 'product-search-view', 
        params: { ProductName: this.searchQuery },
      });
      this.searchQuery = '';
    },
    scrollToTopAndNavigate() {
      window.scrollTo({
        top: 0,
        behavior: 'smooth',
      });
    },
  },
};
</script>

<style scoped>
.navbar {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  background: linear-gradient(to bottom, #1a1d20 90%, rgba(26, 29, 32, 0.95)) !important;
  backdrop-filter: blur(10px);
}

.logo-img {
  height: 2.8rem;
  width: 2.8rem;
  border-radius: 50%;
  object-fit: cover;
  position: relative;
  z-index: 1;
}

.logo-glow {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 100%;
  height: 100%;
  background: var(--color-accent, #00ADB5);
  filter: blur(8px);
  opacity: 0.3;
  border-radius: 50%;
  z-index: 0;
}

.brand-title {
  font-size: 1.5rem;
  font-weight: 700;
  background: linear-gradient(45deg, #00ADB5, #00d4de);
  -webkit-background-clip: text;
  background-clip: text;
  -webkit-text-fill-color: transparent;
}

.nav-search {
  max-width: 500px;
  width: 100%;
}

.search-input {
  background-color: rgba(255, 255, 255, 0.05) !important;
  border: 1px solid rgba(255, 255, 255, 0.1);
  color: #fff !important;
  transition: all 0.3s ease;
}

.search-input:focus {
  background-color: rgba(255, 255, 255, 0.1) !important;
  border-color: var(--color-accent, #00ADB5);
  box-shadow: 0 0 0 0.25rem rgba(0, 173, 181, 0.25);
}

.search-btn {
  padding-left: 1.25rem;
  padding-right: 1.25rem;
}

.avatar-circle {
  width: 32px;
  height: 32px;
  background-color: var(--color-accent, #00ADB5);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
}

.dropdown-menu {
  background-color: #2a2d31;
  border: 1px solid rgba(255, 255, 255, 0.1);
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.2);
}

.dropdown-item {
  color: #fff;
  transition: all 0.2s ease;
}

.dropdown-item:hover {
  background-color: rgba(0, 173, 181, 0.1);
  color: var(--color-accent, #00ADB5);
}

.dropdown-divider {
  border-color: rgba(255, 255, 255, 0.1);
}

.btn {
  transition: all 0.3s ease;
}

.btn-primary {
  background-color: var(--color-accent, #00ADB5);
  border-color: var(--color-accent, #00ADB5);
}

.btn-primary:hover {
  background-color: var(--color-accent-hover, #008e96);
  border-color: var(--color-accent-hover, #008e96);
  transform: translateY(-1px);
}

.btn-outline-primary {
  color: var(--color-accent, #00ADB5);
  border-color: var(--color-accent, #00ADB5);
}

.btn-outline-primary:hover {
  background-color: var(--color-accent, #00ADB5);
  border-color: var(--color-accent, #00ADB5);
  color: white;
  transform: translateY(-1px);
}

@media (max-width: 991.98px) {
  .navbar-collapse {
    padding: 1rem 0;
  }
  
  .nav-search {
    margin: 1rem 0;
  }
}
</style>
