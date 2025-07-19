import axios from 'axios';

export default {
  list() {
    return axios.get('/product');
  },
  getProduct(id) {
    return axios.get(`/product/${id}`);
  },
  search(productName) {
    return axios.get(`/product?productName=${productName}`);
  },
  getProductsByPriceRange(){
    return axios.get('/products/cards')
  }
};
