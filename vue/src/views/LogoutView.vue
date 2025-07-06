<template>
  <div class="logout-message">
    <h1>Logging out...</h1>
    <p>You will be redirected to the login page.</p>
    <div class="loading">
      <div class="loader"></div>
    </div>
  </div>
</template>

<script>
export default {
  created() {
    // Short timeout to show the logout message before redirecting
    setTimeout(() => {
      this.$store.commit('LOGOUT');
      this.$router.push('/login');
    }, 1500);
  },
};
</script>

<style scoped>
.logout-message {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 80vh;
  text-align: center;
  padding: var(--spacing-lg);
}

.logout-message h1 {
  margin-bottom: var(--spacing-md);
  color: var(--color-accent);
}

.logout-message p {
  color: var(--color-text-secondary);
  margin-bottom: var(--spacing-lg);
}

.loader {
  border: 4px solid rgba(255, 255, 255, 0.1);
  border-radius: 50%;
  border-top: 4px solid var(--color-accent);
  width: 40px;
  height: 40px;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
</style>
