// Frontend/src/services/movieService.js
import axios from "axios";

///-----CRIA AXIOS INSTANCE COM BASE URL DA API-----///
export const api = axios.create({
  baseURL: "https://localhost:44351/api",
});

//-----INTERCEPTOR PARA ADICIONAR TOKEN JWT AUTOMAT-----//
api.interceptors.request.use(config => {
  const token = localStorage.getItem("token"); // Pegando token do localStorage
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

//-----FUNCOES PARA CONSUMIR A API-----//
export const getFilmesPopulares = async (page = 1) => {
  const response = await api.get(`/movies/populares?page=${page}`);
  return response.data;
};

export const buscarFilmes = async (query, page = 1) => {
  const response = await api.get(`/movies/buscar?query=${encodeURIComponent(query)}&page=${page}`);
  return response.data;
};

export const getFilmeDetalhes = async (id) => {
  const response = await api.get(`/movies/${id}`);
  return response.data;
};

export const getFilmeCreditos = async (id) => {
  const response = await api.get(`/movies/${id}/creditos`);
  return response.data;
};

export const getFilmeVideos = async (id) => {
  const response = await api.get(`/movies/${id}/videos`);
  return response.data;
};