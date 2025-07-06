<template>
  <footer id="footer" class="footer-component">
    <div class="container">
      <!-- Main Footer Content -->
      <div class="footer-content">
        <div class="row g-4">
          <!-- About Us Column -->
          <div class="col-md-6 col-lg">
            <h5 class="footer-heading">About Us</h5>
            <ul class="footer-links">
              <li><a href="#">About Us Page</a></li>
              <li><a href="#">Our Story</a></li>
              <li><a href="#">Careers</a></li>
              <li><a href="#">Press</a></li>
            </ul>
          </div>
          
          <!-- Customer Service Column -->
          <div class="col-md-6 col-lg">
            <h5 class="footer-heading">Customer Service</h5>
            <ul class="footer-links">
              <li><a href="#">Contact Us</a></li>
              <li><a href="#">FAQs</a></li>
              <li><a href="#">Shipping Information</a></li>
              <li><a href="#">Return Policy</a></li>
            </ul>
          </div>
          
          <!-- Shop Column -->
          <div class="col-md-6 col-lg">
            <h5 class="footer-heading">Shop</h5>
            <ul class="footer-links">
              <li><a href="#">New Arrivals</a></li>
              <li><a href="#">Best Sellers</a></li>
              <li><a href="#">Sale</a></li>
              <li><a href="#">Gift Cards</a></li>
            </ul>
          </div>
          
          <!-- Social Media Column -->
          <div class="col-md-6 col-lg">
            <h5 class="footer-heading">Follow Us</h5>
            <div class="social-icons">
              <a href="#" aria-label="Facebook" class="social-icon-link">
                <i class="bi bi-facebook"></i>
              </a>
              <a href="#" aria-label="Instagram" class="social-icon-link">
                <i class="bi bi-instagram"></i>
              </a>
              <a href="#" aria-label="Twitter" class="social-icon-link">
                <i class="bi bi-twitter-x"></i>
              </a>
              <a href="#" aria-label="Pinterest" class="social-icon-link">
                <i class="bi bi-pinterest"></i>
              </a>
            </div>
          </div>
          
          <!-- Newsletter Column -->
          <div class="col-lg-4 mt-md-0 mt-4">
            <h5 class="footer-heading">Newsletter</h5>
            <p class="newsletter-text">Subscribe to our newsletter for the latest updates and exclusive offers.</p>
            <div class="newsletter-form">
              <div class="input-group">
                <input 
                  type="email" 
                  v-model="email" 
                  class="form-control" 
                  placeholder="Enter your email" 
                  aria-label="Email for newsletter"
                >
                <button @click="subscribe" class="btn newsletter-btn" type="button">
                  <i class="bi bi-send-fill me-2"></i>Subscribe
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Footer Bottom -->
      <div class="footer-bottom">
        <p class="copyright">© 2025 Market. All Rights Reserved.</p>
        <ul class="footer-legal">
          <li><a href="#">Privacy Policy</a></li>
          <li><a href="#">Terms of Service</a></li>
          <li><a href="#">Accessibility</a></li>
        </ul>
      </div>
    </div>
  </footer>
</template>

<script>
export default {
  name: 'FooterComponent',
  data() {
    return {
      email: '',
    };
  },
  methods: {
    subscribe() {
      if (!this.email.trim()) {
        return;
      }
      
      // Handle subscription logic here
      alert(`Subscribed with ${this.email}`);
      this.email = '';
    },
  },
};
</script>

<style scoped>
.footer-component {
  background-color: var(--color-dark-secondary);
  color: var(--color-text-light);
  padding: 4rem 0 2rem;
  margin-top: auto;
}

.footer-content {
  margin-bottom: 3rem;
}

.footer-heading {
  color: var(--color-accent);
  font-size: 1.1rem;
  font-weight: 600;
  margin-bottom: 1.25rem;
  position: relative;
  padding-bottom: 0.75rem;
}

.footer-heading::after {
  content: "";
  position: absolute;
  bottom: 0;
  left: 0;
  width: 40px;
  height: 2px;
  background-color: var(--color-accent);
}

.footer-links {
  list-style: none;
  padding: 0;
  margin: 0;
}

.footer-links li {
  margin-bottom: 0.75rem;
}

.footer-links a {
  color: var(--color-text-muted);
  text-decoration: none;
  transition: color 0.2s ease, transform 0.2s ease;
  display: inline-block;
}

.footer-links a:hover {
  color: var(--color-accent);
  transform: translateX(5px);
}

.social-icons {
  display: flex;
  gap: 1rem;
}

.social-icon-link {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 38px;
  height: 38px;
  border-radius: 50%;
  background-color: rgba(255, 255, 255, 0.05);
  color: var(--color-text-light);
  transition: all 0.3s ease;
}

.social-icon-link i {
  font-size: 1.25rem;
}

.social-icon-link:hover {
  background-color: var(--color-accent);
  color: var(--color-dark);
  transform: translateY(-3px);
}

.newsletter-text {
  color: var(--color-text-muted);
  font-size: 0.9rem;
  margin-bottom: 1rem;
  line-height: 1.5;
}

.newsletter-form .form-control {
  background-color: var(--color-dark);
  border: 1px solid rgba(255, 255, 255, 0.1);
  color: var(--color-text-light);
  padding: 0.75rem 1rem;
  border-radius: var(--radius-sm) 0 0 var(--radius-sm);
}

.newsletter-form .form-control:focus {
  box-shadow: none;
  border-color: var(--color-accent);
}

.newsletter-btn {
  background-color: var(--color-accent);
  color: var(--color-dark);
  font-weight: 500;
  border: none;
  padding: 0 1.25rem;
  border-radius: 0 var(--radius-sm) var(--radius-sm) 0;
  transition: background-color 0.2s ease;
}

.newsletter-btn:hover {
  background-color: var(--color-accent-dark);
}

.footer-bottom {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-items: center;
  padding-top: 2rem;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
}

.copyright {
  color: var(--color-text-muted);
  font-size: 0.85rem;
  margin-bottom: 1rem;
}

.footer-legal {
  display: flex;
  flex-wrap: wrap;
  gap: 1.5rem;
  list-style: none;
  padding: 0;
  margin: 0;
}

.footer-legal li a {
  color: var(--color-text-muted);
  font-size: 0.85rem;
  text-decoration: none;
  transition: color 0.2s ease;
}

.footer-legal li a:hover {
  color: var(--color-accent);
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .footer-bottom {
    flex-direction: column;
    align-items: flex-start;
  }
  
  .footer-legal {
    margin-bottom: 1rem;
  }
}
</style>
