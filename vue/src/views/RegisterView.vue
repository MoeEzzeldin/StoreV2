<template>
  <div id="register" class="register-container">
    <form @submit.prevent="register" class="form-container">
      <h1 class="form-title">Create Account</h1>
      <div role="alert" v-if="registrationErrors" class="alert alert-danger">
        {{ registrationErrorMsg }}
      </div>
      <div class="form-input-group">
        <label for="username">Username</label>
        <input type="text" id="username" v-model="user.username" required autofocus @input="clearErrors" />
      </div>
      <div class="form-input-group">
        <label for="password">Password</label>
        <input type="password" id="password" v-model="user.password" required @input="clearErrors" />
      </div>
      <div class="form-input-group">
        <label for="confirmPassword">Confirm Password</label>
        <input type="password" id="confirmPassword" v-model="user.confirmPassword" required @input="clearErrors" />
      </div>
      <button type="submit" class="btn btn-primary w-100">Create Account</button>
      <p class="form-link">
        <router-link :to="{ name: 'login' }">Already have an account? Log in.</router-link>
      </p>
    </form>
  </div>
</template>

<script>
import authService from '../services/AuthService';

export default {
  data() {
    return {
      user: {
        username: '',
        password: '',
        confirmPassword: '',
        role: 'user',
      },
      registrationErrors: false,
      registrationErrorMsg: 'There were problems registering this user.',
    };
  },
  methods: {
    register() {
      if (this.user.password !== this.user.confirmPassword) {
        this.registrationErrors = true;
        this.registrationErrorMsg = 'Password & Confirm Password do not match.';
      } else {
        authService
          .register(this.user)
          .then((response) => {
            if (response.status === 201) {
              this.$router.push({
                path: '/login',
                query: { registration: 'success' },
              });
            }
          })
          .catch((error) => {
            const response = error.response;
            this.registrationErrors = true;
            if (response && response.status === 400) {
              this.registrationErrorMsg = 'Bad Request: Validation Errors';
            }
          });
      }
    },
    clearErrors() {
      this.registrationErrors = false;
      this.registrationErrorMsg = 'There were problems registering this user.';
    },
  },
};
</script>

<style scoped>
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  padding: var(--spacing-xl) 0;
}

.form-container {
  width: 100%;
  max-width: 400px;
  padding: var(--spacing-lg);
  margin: 0 auto;
  border-radius: var(--radius-lg);
  background-color: var(--color-dark-secondary);
  box-shadow: var(--shadow-lg);
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
