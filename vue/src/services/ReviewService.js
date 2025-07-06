import axios from 'axios';

export default {
  list() {
    return axios.get('/review');
  },
  productReviewList(productId) {
    return axios.get(`/review/0?productId=${productId}`);
  },

  avg(productId) {
    // need checked
    return axios.get(`/review/${productId}/averageRating`);
  },

  addAReview(review) {
    return axios.post('/review', review);
  },
};
