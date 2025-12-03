import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getFilmeDetalhes, getFilmeCreditos, getFilmeVideos } from "../services/movieService";

const FilmeDetalhes = () => {
  const { id } = useParams();
  const [filme, setFilme] = useState(null);
  const [creditos, setCreditos] = useState([]);
  const [trailer, setTrailer] = useState(null);

  useEffect(() => {
    const fetchFilmeDetalhes = async () => {
      try {
        const detalhes = await getFilmeDetalhes(id);
        setFilme(detalhes);

        const elenco = await getFilmeCreditos(id);
        setCreditos(elenco.cast.map((ator) => ator.name));

        const filmesVideos = await getFilmeVideos(id);
        const primeiroTrailer = filmesVideos.results.find(
          (video) => video.type === "Trailer" && video.site === "YouTube"
        );
        setTrailer(primeiroTrailer);
      } catch (error) {
        console.error("Erro ao buscar detalhes do filme:", error);
      }
    };

    fetchFilmeDetalhes();
  }, [id]);

  if (!filme) return <p>Carregando...</p>;

  return (
    <div
      style={{
        maxWidth: "1000px",
        margin: "40px auto",
        padding: "20px",
        backgroundColor: "#1e1e1e",
        color: "#fff",
        borderRadius: "10px",
        boxShadow: "0 4px 20px rgba(0,0,0,0.5)",
        fontFamily: "Arial, sans-serif",
      }}
    >
      <div style={{ display: "flex", flexWrap: "wrap", gap: "20px" }}>
        <img
          src={`https://image.tmdb.org/t/p/w300${filme.poster_path}`}
          alt={filme.title}
          style={{ borderRadius: "8px", flexShrink: 0 }}
        />

        <div style={{ flex: 1, minWidth: "300px" }}>
          <h1 style={{ marginBottom: "10px" }}>{filme.title}</h1>
          <p><strong>Nota:</strong> {filme.vote_average}</p>
          <p><strong>Data de lançamento:</strong> {filme.release_date}</p>
          <p><strong>Gêneros:</strong> {filme.genres.map(g => g.name).join(", ")}</p>
        </div>
      </div>

      <div style={{ marginTop: "20px" }}>
        <h2>Sinopse</h2>
        <p>{filme.overview}</p>
      </div>

      {trailer && (
        <div style={{ marginTop: "20px" }}>
          <h2>Trailer</h2>
          <iframe
            width="100%"
            height="500px"
            src={`https://www.youtube.com/embed/${trailer.key}`}
            title={trailer.name}
            frameBorder="0"
            allowFullScreen
            style={{ borderRadius: "8px" }}
          ></iframe>
        </div>
      )}

      {creditos.length > 0 && (
        <div style={{ marginTop: "20px" }}>
          <h2>Elenco</h2>
          <ul style={{ listStyle: "none", padding: 0, display: "flex", flexWrap: "wrap", gap: "10px" }}>
            {creditos.map((nome, index) => (
              <li
                key={index}
                style={{
                  backgroundColor: "#2e2e2e",
                  padding: "5px 10px",
                  borderRadius: "5px",
                }}
              >
                {nome}
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default FilmeDetalhes;
