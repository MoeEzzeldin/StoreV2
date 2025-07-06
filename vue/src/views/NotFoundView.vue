<template>
  <div class="not-found">
    <div class="not-found-content">
      <h1 class="not-found-title">404</h1>
      <h2>Page Not Found</h2>
      <p>The page you're looking for doesn't exist or has been moved.</p>
      <router-link to="/home" class="btn">Return to Home</router-link>
    </div>
  </div>
</template>

<style scoped>
.not-found {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 80vh;
  padding: var(--spacing-lg);
  text-align: center;
}

.not-found-content {
  max-width: 600px;
  background-color: var(--color-dark-secondary);
  padding: var(--spacing-xl);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-lg);
}

.not-found-title {
  font-size: 6rem;
  font-weight: 700;
  margin-bottom: var(--spacing-md);
  color: var(--color-accent);
}

.not-found h2 {
  font-size: 2rem;
  margin-bottom: var(--spacing-md);
}

.not-found p {
  color: var(--color-text-secondary);
  margin-bottom: var(--spacing-lg);
  font-size: 1.1rem;
}
</style>