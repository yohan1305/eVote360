using Evote365.Core.Application.ViewModels.Administrador.AsignacionPartido;
using Evote365.Core.Application.ViewModels;
using Evote365.Core.Application.Dtos.Dirigente.AlianzaPolitica;

namespace Evote365.Core.Application.Interfaces.Dirigente
{
    public interface IAlianzaPoliticaService : IGenericService<AlianzaSolicitudEnviadaDto>
    {
        //  Listado 1: Solicitudes recibidas (pendientes de respuesta)
        Task<List<AlianzaSolicitudRecibidaDto>> GetSolicitudesRecibidasAsync(int partidoId);

        //  Listado 2: Solicitudes enviadas por el partido logueado
        Task<List<AlianzaSolicitudEnviadaDto>> GetSolicitudesEnviadasAsync(int partidoId);

        //  Listado 3: Alianzas vigentes
        Task<List<AlianzaVigenteDto>> GetAlianzasVigentesAsync(int partidoId);

        //  Validaciones para crear solicitud
        Task<List<Evote365.Core.Application.ViewModels.OpcionItemViewModel>> GetPartidosDisponiblesParaAlianza(int partidoId);

        //  Crear solicitud
        Task<bool> CrearSolicitudAsync(int partidoSolicitanteId, int partidoReceptorId);

        //  Confirmar aceptación
        Task<ConfirmarAccionAlianzaDto?> GetConfirmacionAceptarAsync(int solicitudId);
        Task<bool> AceptarSolicitudAsync(int solicitudId);

        //  Confirmar rechazo
        Task<ConfirmarAccionAlianzaDto?> GetConfirmacionRechazoAsync(int solicitudId);
        Task<bool> RechazarSolicitudAsync(int solicitudId);

        //  Confirmar eliminación
        Task<ConfirmarAccionAlianzaDto?> GetConfirmacionEliminarAsync(int solicitudId);
        Task<bool> EliminarSolicitudAsync(int solicitudId);

        //  Validación de bloqueo por elección activa
        Task<bool> CanModifyAsync();

         Task<List<int>> GetAliadosVigentesIdsAsync(int partidoId);
    }
}
