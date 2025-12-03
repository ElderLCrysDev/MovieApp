const API_URL = "https://localhost:44351";

//-----LOGIN DO USUARIO-----//
export async function apiLogin(username, password) {
  const response = await fetch(`${API_URL}/api/auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ usuario: username, senha: password }),
  });

  if (!response.ok) throw new Error("Usuário ou senha inválidos");
  return response.json();
}

//-----FILMES POPULARES-----//
export async function apiGetFilmesPopulares(page = 1) {
  const token = localStorage.getItem("token");
  const response = await fetch(`${API_URL}/api/movies/populares?page=${page}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  if (!response.ok) throw new Error("Erro ao buscar filmes");
  return response.json();
}

//-----BUSCA DE FILMES-----//
export async function apiBuscarFilmes(query, page = 1) {
  const token = localStorage.getItem("token");
  const response = await fetch(
    `${API_URL}/api/movies/buscar?query=${encodeURIComponent(query)}&page=${page}`,
    { headers: { Authorization: `Bearer ${token}` } }
  );
  if (!response.ok) throw new Error("Erro ao buscar filmes");
  return response.json();
}

//-----DETALHES DE UM FILME-----//
export async function apiGetDetalhesFilme(id) {
  const token = localStorage.getItem("token");
  const response = await fetch(`${API_URL}/api/movies/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  if (!response.ok) throw new Error("Erro ao buscar detalhes do filme");
  return response.json();
}

//-----CREDITOS DE UM FILME-----//
export async function apiGetCreditosFilme(id) {
  const token = localStorage.getItem("token");
  const response = await fetch(`${API_URL}/api/movies/${id}/creditos`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  if (!response.ok) throw new Error("Erro ao buscar créditos do filme");
  return response.json();
}

//-----TRAILER DO FILME-----//
export async function apiGetVideosFilme(id) {
  const token = localStorage.getItem("token");
  const response = await fetch(`${API_URL}/api/movies/${id}/videos`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  if (!response.ok) throw new Error("Erro ao buscar vídeos do filme");
  return response.json();
}
