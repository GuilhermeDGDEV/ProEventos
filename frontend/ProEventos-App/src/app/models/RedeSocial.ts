import { Evento } from "./Evento";
import { Palestrante } from "./Palestrante";

export interface RedeSocial {
    id: number;
    nome: string;
    url: string;
    eventoId?: number;
    evento: Evento;
    palestranteId?: number;
    palestrante: Palestrante;
}