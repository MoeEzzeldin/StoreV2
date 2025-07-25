import { createRouter as createRouter, createWebHistory } from 'vue-router';
import { useStore } from 'vuex';

// Import components
import HomeView from '../views/HomeView.vue';
import LoginView from '../views/LoginView.vue';
import LogoutView from '../views/LogoutView.vue';
import RegisterView from '../views/RegisterView.vue';
import NotFoundView from '../views/NotFoundView.vue';
import ProductView from '../views/ProductView.vue';
import ProductSearchView from '../views/ProductSearchView.vue';

/**
 * The Vue Router is used to "direct" the browser to render a specific view component
 * inside of App.vue depending on the URL.
 *
 * It also is used to detect whether or not a route requires the user to have first authenticated.
 * If the user has not yet authenticated (and needs to) they are redirected to /login
 * If they have (or don't need to) they're allowed to go about their way.
 */
const routes = [
  // {
  //   path: '/',
  //   name: 'login'
  // },
  //home--->products
  {
    path: '/home',
    name: 'home',
    component: HomeView,
    meta: {
      requiresAuth: false,
    },
  },
  //Register---->home/products
  {
    path: '/register',
    name: 'register',
    component: RegisterView,
    meta: {
      requiresAuth: false,
    },
  },
  // Login---->Home---->Products
  {
    path: '/login',
    name: 'login',
    component: LoginView,
    meta: {
      requiresAuth: false,
    },
  },
  //logout---->/home/products
  {
    path: '/',
    name: 'logout',
    component: LogoutView,
    meta: {
      requiresAuth: false,
    },
  },
  //ProductView
  {
    path: '/product/:id',
    name: 'product-view',
    component: ProductView,
    meta: {
      requiresAuth: true,
    },
  },
  //ProductSearchView
  {
    path: '/product?productName=:ProductName',
    name: 'product-search-view',
    component: ProductSearchView,
    meta: {
      requiresAuth: false,
    },
  },
  // NotFound
  {
    path: '/:pathMatch(.*)*',
    name: 'notFound',
    component: NotFoundView,
  },
];

// Create the router
const router = createRouter({
  history: createWebHistory(),
  routes: routes,
});

router.beforeEach((to) => {
  // Get the Vuex store
  const store = useStore();

  // Determine if the route requires Authentication
  const requiresAuth = to.matched.some((x) => x.meta.requiresAuth);

  // If it does and they are not logged in, send the user to "/login"
  if (requiresAuth && store.state.token === '') {
    console.log('Not logged in. Redirected to login view.');
    return { name: 'login' };
  }
  // Otherwise, do nothing and they'll go to their next destination
});

export default router;
