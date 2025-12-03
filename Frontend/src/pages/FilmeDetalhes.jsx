import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getFilmeDetalhes, getFilmeCreditos, getFilmeVideos } from "../services/movieService";

const FilmeDetalhes = () => {
  const { id } = useParams();
  const [filme, setFilme] = useState(null);
  const [creditos, setCreditos] = useState([]);
  const [videos, setVideos] = useState([]);

  useEffect(() => {
    const fetchFilmeDetalhes = async () => {
      try {
        const detalhes = await getFilmeDetalhes(id);
        setFilme(detalhes);

        const elenco = await getFilmeCreditos(id);
        setCreditos(elenco.cast.slice(0, 5)); //-----PEGA APENAS OS 5 PRIMEIROS ATORES-----//

        const filmesVideos = await getFilmeVideos(id);
        setVideos(filmesVideos.results);
      } catch (error) {
        console.error("Erro ao buscar detalhes do filme:", error);
      }
    };

    fetchFilmeDetalhes();
  }, [id]);

  if (!filme) return <p>Carregando...</p>;

  return (
    <div>
      <h1>{filme.title}</h1>
      <p>{filme.overview}</p>

      <h2>Elenco</h2>
      <ul>
        {creditos.map((ator) => (
          <li key={ator.id}>
            <img
              src={`https://image.tmdb.org/t/p/w200${ator.profile_path}`}
              alt={ator.name}
            />
            <p>{ator.name}</p>
          </li>
        ))}
      </ul>

      <h2>Trailers</h2>
      {videos.map((video) => (
        <iframe
          key={video.id}
          width="560"
          height="315"
          src={`https://www.youtube.com/embed/${video.key}`}
          title={video.name}
          frameBorder="0"
          allowFullScreen
        ></iframe>
      ))}
    </div>
  );
};

export default FilmeDetalhes;
