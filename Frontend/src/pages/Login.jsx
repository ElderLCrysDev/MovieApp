import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { apiLogin } from "../services/api";
import { TextField, Button, Box, Typography, Container } from "@mui/material";

export default function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  async function handleSubmit(e) {
    e.preventDefault(); //-----PREVINE REFRESH DA PAGINA-----//
    setLoading(true);
    setError("");

    try {
      const result = await apiLogin(username, password);

      if (result.token) {
        localStorage.setItem("token", result.token);
      }

      navigate("/home");

    } catch (err) {
      setError("Usuário ou senha incorretos");
    } finally {
      setLoading(false);
    }
  }

  return (
    <Container maxWidth="xs">
      <Box
        mt={8}
        component="form"
        onSubmit={handleSubmit}
        display="flex"
        flexDirection="column"
        gap={2}
      >
        <Typography variant="h5" textAlign="center">
          Login
        </Typography>

        <TextField
          label="Usuário"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />

        <TextField
          type="password"
          label="Senha"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />

        {error && (
          <Typography color="error" textAlign="center">
            {error}
          </Typography>
        )}

        <Button
          type="submit"
          variant="contained"
          disabled={loading}
        >
          {loading ? "Entrando..." : "Entrar"}
        </Button>
      </Box>
    </Container>
  );
}
