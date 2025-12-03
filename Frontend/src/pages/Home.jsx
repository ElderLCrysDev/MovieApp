import { useEffect, useState, useRef } from "react";
import { useNavigate } from "react-router-dom";
import {
  TextField,
  Box,
  Typography,
  Container,
  Modal,
  Card,
  CardMedia,
  CardContent,
  Button,
} from "@mui/material";

import {
  getFilmesPopulares,
  buscarFilmes,
  getFilmeDetalhes,
  getFilmeCreditos,
  getFilmeVideos,
} from "../services/movieService";

export default function Home() {
  const [filmes, setFilmes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [query, setQuery] = useState("");
  const [searchTerm, setSearchTerm] = useState("");
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const [selectedFilme, setSelectedFilme] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [detalhes, setDetalhes] = useState(null);
  const [creditos, setCreditos] = useState(null);
  const [video, setVideo] = useState(null);

  const hoverTimeout = useRef(null);
  const navigate = useNavigate();

  useEffect(() => {
    carregarFilmes(page);
  }, [page]);

  async function carregarFilmes(p = 1) {
    setLoading(true);
    try {
      let data;
      if (searchTerm) {
        data = await buscarFilmes(searchTerm, p);
      } else {
        data = await getFilmesPopulares(p);
      }
      setFilmes(data.results);
      setTotalPages(data.total_pages || 1);
    } catch (err) {
      console.error("Erro ao buscar filmes", err);
      alert("Erro ao carregar filmes. Verifique se você está logado.");
    } finally {
      setLoading(false);
    }
  }

  async function handleSearch(e) {
    e.preventDefault();
    if (!query.trim()) {
      setSearchTerm("");
      setPage(1);
      return carregarFilmes(1);
    }
    setSearchTerm(query);
    setPage(1);
    carregarFilmes(1);
  }

  function handleMouseEnter(filme) {
    hoverTimeout.current = setTimeout(async () => {
      setSelectedFilme(filme);
      setModalOpen(true);

      try {
        const [detalhesData, creditosData, videosData] = await Promise.all([
          getFilmeDetalhes(filme.id),
          getFilmeCreditos(filme.id),
          getFilmeVideos(filme.id),
        ]);
        setDetalhes(detalhesData);
        setCreditos(creditosData);
        setVideo(videosData.results?.[0] || null);
      } catch (err) {
        console.error("Erro ao buscar detalhes do filme", err);
      }
    }, 1000);
  }

  function handleMouseLeave() {
    clearTimeout(hoverTimeout.current);
  }

  function handleCardClick(filme) {
    navigate(`/filme/${filme.id}`);
  }

  function handleCloseModal() {
    setModalOpen(false);
    setSelectedFilme(null);
    setDetalhes(null);
    setCreditos(null);
    setVideo(null);
  }

  function handlePageChange(newPage) {
    if (newPage < 1 || newPage > totalPages) return;
    setPage(newPage);
  }

  return (
    <Container>
      {/* BARRA DE PESQUISAS */}
      <Box
        component="form"
        onSubmit={handleSearch}
        display="flex"
        justifyContent="center"
        my={3}
        gap={2}
      >
        <TextField
          label="Buscar filmes"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
        />
        <button type="submit">Buscar</button>
      </Box>

      {/* TITULO */}
      {!searchTerm ? (
        <Typography variant="h4" textAlign="center" mb={3}>
          Filmes Populares
        </Typography>
      ) : (
        <Typography variant="h5" textAlign="center" mb={3}>
          Resultados da busca por "{searchTerm}"
        </Typography>
      )}

      {/* LOAD */}
      {loading && (
        <Typography textAlign="center" mb={2}>
          Carregando filmes...
        </Typography>
      )}
      {!loading && filmes.length === 0 && (
        <Typography textAlign="center">Nenhum filme encontrado.</Typography>
      )}

      {/* LISTA DE FILMES */}
      <Box display="flex" flexWrap="wrap" gap={2} justifyContent="center">
        {filmes.map((filme) => (
          <Card
            key={filme.id}
            sx={{
              width: 200,
              cursor: "pointer",
              transition: "filter 0.3s",
              "&:hover": { filter: "brightness(85%)" },
            }}
            onMouseEnter={() => handleMouseEnter(filme)}
            onMouseLeave={handleMouseLeave}
            onClick={() => handleCardClick(filme)}
          >
            {filme.poster_path ? (
              <CardMedia
                component="img"
                image={`https://image.tmdb.org/t/p/w200${filme.poster_path}`}
                alt={filme.title}
              />
            ) : (
              <Box
                sx={{
                  height: 300,
                  display: "flex",
                  justifyContent: "center",
                  alignItems: "center",
                  bgcolor: "#ccc",
                }}
              >
                Sem imagem disponível
              </Box>
            )}
            <CardContent>
              <Typography variant="subtitle1">
                {filme.title} ({filme.release_date?.slice(0, 4)})
              </Typography>
            </CardContent>
          </Card>
        ))}
      </Box>

      {/* PAGINACAO */}
      {totalPages > 1 && (
        <Box display="flex" justifyContent="center" mt={3} gap={1}>
          <Button onClick={() => handlePageChange(page - 1)} disabled={page === 1}>
            Anterior
          </Button>
          <Typography variant="body1" sx={{ alignSelf: "center" }}>
            {page} / {totalPages}
          </Typography>
          <Button
            onClick={() => handlePageChange(page + 1)}
            disabled={page === totalPages}
          >
            Próxima
          </Button>
        </Box>
      )}

      {/* MODAL */}
      <Modal open={modalOpen} onClose={handleCloseModal}>
        <Box
          sx={{
            position: "absolute",
            top: "50%",
            left: "50%",
            transform: "translate(-50%, -50%)",
            bgcolor: "background.paper",
            p: 3,
            maxWidth: 600,
            width: "90%",
            maxHeight: "90%",
            overflowY: "auto",
          }}
        >
          {detalhes && (
            <>
              <Typography variant="h5">{detalhes.title}</Typography>
              <Typography variant="body2" mb={2}>
                {detalhes.overview || "Sem descrição disponível."}
              </Typography>
            </>
          )}

          {creditos && (
            <>
              <Typography variant="subtitle1">Elenco:</Typography>
              <Typography variant="body2" mb={2}>
                {creditos.cast?.slice(0, 5).map((c) => c.name).join(", ") ||
                  "Não disponível"}
              </Typography>
            </>
          )}

          <Box mb={2}>
            <Typography variant="subtitle1">Trailer:</Typography>
            {video ? (
              <iframe
                width="100%"
                height="300"
                src={`https://www.youtube.com/embed/${video.key}`}
                title="Vídeo"
                frameBorder="0"
                allowFullScreen
              />
            ) : (
              <Typography>SEM TRAILER DISPONÍVEL</Typography>
            )}
          </Box>
        </Box>
      </Modal>
    </Container>
  );
}
