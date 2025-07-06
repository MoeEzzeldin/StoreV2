<template>
  <div id="login" class="login-container">
    <span class="background-img"></span>
    <form @submit.prevent="login" class="form-container">
      <h1 class="form-title">Please Sign In</h1>
      <div role="alert" v-if="invalidCredentials" class="alert alert-danger">
        Invalid username and password!
      </div>
      <div
        role="alert"
        v-if="this.$route.query.registration"
        class="alert alert-success"
      >
        Thank you for registering, please sign in.
      </div>
      <div class="form-input-group">
        <label for="username">Username</label>
        <input
          type="text"
          id="username"
          v-model="user.username"
          required
          autofocus
        />
      </div>
      <div class="form-input-group">
        <label for="password">Password</label>
        <input
          type="password"
          id="password"
          v-model="user.password"
          required
        />
      </div>
      <button @click="login" type="submit" class="btn btn-primary w-100">
        Sign In
      </button>

      <p class="form-link">
        <router-link :to="{ name: 'register' }"
          >Need an account? Sign up.</router-link
        >
      </p>
    </form>
  </div>

</template>

<script>
import { RouterLink } from 'vue-router';
import authService from '../services/AuthService';


export default {
  data() {
    return {
      user: {
        username: '',
        password: '',
      },
      invalidCredentials: false,
    };
  },
  methods: {
    login() {
      authService
        .login(this.user)
        .then((response) => {
          if (response.status === 200) {
            this.$store.commit('SET_AUTH_TOKEN', response.data.token);
            this.$store.commit('SET_USER', response.data.user);
            this.$router.push('/home');
          }
        })
        .catch((error) => {
          if (error.response && error.response.status === 401) {
            this.invalidCredentials = true;
          }
        });
    },

  },
  created() {
  },
};
</script>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  position: relative;
}

.background-img {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  z-index: -2;
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
  background-image: url('../assets/vecteezy_woman-holding-multi-coloured-shopping-bags_2037496.jpg');
  opacity: 0.5;
}

.login-container::before {
  content: '';
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.6);
  z-index: -1;
}

.form-container {
  width: 100%;
  max-width: 400px;
  padding: var(--spacing-lg);
  margin: 0 auto;
  border-radius: var(--radius-lg);
  background-color: rgba(var(--bs-dark-rgb), 0.85);
  backdrop-filter: blur(10px);
  box-shadow: var(--shadow-lg);
  z-index: 1;
}

.form-title {
  margin-bottom: var(--spacing-lg);
  text-align: center;
  color: var(--color-accent);
  font-size: 1.8rem;
}

.form-input-group {
  margin-bottom: var(--spacing-md);
}

.form-link {
  text-align: center;
  margin-top: var(--spacing-md);
}

.form-link a {
  color: var(--color-accent);
  transition: color var(--transition-fast);
}

.form-link a:hover {
  color: var(--color-accent-dark);
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .form-container {
    max-width: 90%;
    margin: 0 20px;
  }
}
</style>
