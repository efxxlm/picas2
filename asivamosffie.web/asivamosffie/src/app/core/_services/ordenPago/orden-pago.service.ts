import { CommonService, Dominio, Respuesta } from 'src/app/core/_services/common/common.service';
import { environment } from './../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import humanize from 'humanize-plus';
import { TipoAportanteCodigo, TipoAportanteDominio } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Injectable({
  providedIn: 'root'
})
export class OrdenPagoService {

    private urlApi = `${ environment.apiUrl }/GenerateSpinOrder`;
    listaTipoAportantes: TipoAportanteDominio = TipoAportanteCodigo;

    constructor(
        private http: HttpClient,
        private commonSvc: CommonService )
    { }

    getListOrdenGiro( pMenuId: number ) {
        return this.http.get<any[]>( `${ this.urlApi }/GetListOrdenGiro?pMenuId=${ pMenuId }` );
    }

    getSolicitudPagoBySolicitudPagoId( SolicitudPagoId: number , esSolicitudPago: boolean) {
        return this.http.get( `${ this.urlApi }/GetSolicitudPagoBySolicitudPagoId?SolicitudPagoId=${ SolicitudPagoId }&esSolicitudPago=${ esSolicitudPago }` );
    }

    getInfoPlantilla( pOrdenGiroId: number ) {
        return this.http.get( `${ this.urlApi }/GetInfoPlantilla?pOrdenGiroId=${ pOrdenGiroId }` );
    }

    createEditOrdenGiro( pOrdenGiro: any ) {
        return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditOrdenGiro`, pOrdenGiro );
    }

    getFuentesDeRecursosPorAportanteId( pAportanteId: number ) {
        return this.http.get<any[]>( `${ this.urlApi }/GetFuentesDeRecursosPorAportanteId?pAportanteId=${ pAportanteId }` );
    }

    asyncGetFuentesDeRecursosPorAportanteId( pAportanteId: number ) {
        return new Promise<any[]>( resolve => {
            this.http.get<any[]>( `${ this.urlApi }/GetFuentesDeRecursosPorAportanteId?pAportanteId=${ pAportanteId }` )
                .subscribe( fuenteRecursos => resolve( fuenteRecursos ) );
        } )
    }

    changueStatusOrdenGiro( pOrdenGiro: any ) {
        return this.http.post<Respuesta>( `${ environment.apiUrl }/RegisterValidateSpinOrder/ChangueStatusOrdenGiro`, pOrdenGiro );
    }

    deleteOrdenGiroDetalleDescuentoTecnica( pOrdenGiroDetalleDescuentoTecnicaId ) {
        return this.http.post<Respuesta>( `${ this.urlApi }/DeleteOrdenGiroDetalleDescuentoTecnica?pOrdenGiroDetalleDescuentoTecnicaId=${ pOrdenGiroDetalleDescuentoTecnicaId }`, '' );
    }

    deleteOrdenGiroDetalleDescuentoTecnicaAportante( pOrdenGiroDetalleDescuentoTecnicaAportanteId: number ) {
        return this.http.post<Respuesta>( `${ this.urlApi }/DeleteOrdenGiroDetalleDescuentoTecnicaAportante?pOrdenGiroDetalleDescuentoTecnicaAportanteId=${ pOrdenGiroDetalleDescuentoTecnicaAportanteId }`, '' );
    }

    deleteOrdenGiroDetalleTerceroCausacionAportante( pOrdenGiroDetalleTerceroCausacionAportanteId: number ) {
        return this.http.post<Respuesta>( `${ this.urlApi }/DeleteOrdenGiroDetalleTerceroCausacionAportante?pOrdenGiroDetalleTerceroCausacionAportanteId=${ pOrdenGiroDetalleTerceroCausacionAportanteId }`, '' );
    }

    deleteOrdenGiroDetalleTerceroCausacionDescuento( pOrdenGiroDetalleTerceroCausacionDescuentoId: number[] ) {
        return this.http.post<Respuesta>( `${ this.urlApi }/DeleteOrdenGiroDetalleTerceroCausacionDescuento`, pOrdenGiroDetalleTerceroCausacionDescuentoId );
    }

    deleteOrdenGiroDetalleDescuentoTecnicaByConcepto( pOrdenGiroDetalleDescuentoTecnicaId: number, pConceptoPagoCodigo: string ) {
      return this.http.post<Respuesta>( `${ this.urlApi }/DeleteOrdenGiroDetalleDescuentoTecnicaByConcepto?pOrdenGiroDetalleDescuentoTecnicaId=${ pOrdenGiroDetalleDescuentoTecnicaId }&pConceptoPagoCodigo=${ pConceptoPagoCodigo }`, '' );
    }

    async getAportantes( solicitudPago: any ) {
        if ( solicitudPago !== undefined ) {
            // constantes y variables
            const contratacionProyecto: any[] = solicitudPago.contratoSon.contratacion.contratacionProyecto;
            const aportantes: any[] = [];
            const listaNombreAportante: { tipoAportanteId: number, cofinanciacionAportanteId: number, nombreAportante: string }[] = [];
            let tieneAportanteFfie: boolean;
            let tieneAportanteEt: boolean;
            let tieneAportanteTercero: boolean;

            // Get lista de aportantes
            for ( const proyecto of contratacionProyecto ) {
                for ( const aportante of proyecto.contratacionProyectoAportante ) {
                    aportantes.push( aportante );
                }
            }

            // verificar si en la lista de aportante, tiene aportante de tipo FFIE
            const aportanteFfie = aportantes.find( aportante => aportante.cofinanciacionAportante.tipoAportanteId === this.listaTipoAportantes.ffie );
            if ( aportanteFfie !== undefined ) {
                const tieneAportante = listaNombreAportante.includes( aportanteFfie.cofinanciacionAportanteId );

                if ( tieneAportante === false ) {
                    listaNombreAportante.push(
                        {
                            tipoAportanteId: aportanteFfie.cofinanciacionAportante.tipoAportanteId,
                            cofinanciacionAportanteId: aportanteFfie.cofinanciacionAportanteId,
                            nombreAportante: 'FFIE'
                        }
                    )
                }

                tieneAportanteFfie = true;
            } else {
                tieneAportanteFfie = false;
            }

            // verificar si en la lista de aportante, tiene aportante de tipo ET
            const aportanteEt = aportantes.find( aportante => aportante.cofinanciacionAportante.tipoAportanteId === this.listaTipoAportantes.et );
            if ( aportanteEt !== undefined ) {
                const tieneAportante = listaNombreAportante.includes( aportanteEt.cofinanciacionAportanteId );

                if ( tieneAportante === false ) {
                    let nombreAportante: string;

                    if ( aportanteEt.cofinanciacionAportante.departamento !== undefined && aportanteEt.cofinanciacionAportante.municipio === undefined ) {
                        nombreAportante = `Gobernaci??n de ${ this.firstLetterUpperCase( aportanteEt.cofinanciacionAportante.departamento.descripcion ) }`;
                    }

                    if ( aportanteEt.cofinanciacionAportante.departamento !== undefined && aportanteEt.cofinanciacionAportante.municipio !== undefined ) {
                        nombreAportante = `Alcald??a de ${ this.firstLetterUpperCase( aportanteEt.cofinanciacionAportante.municipio.descripcion ) }`;
                    }

                    if ( aportanteEt.cofinanciacionAportante.departamento === undefined && aportanteEt.cofinanciacionAportante.municipio !== undefined ) {
                        nombreAportante = `Alcald??a de ${ this.firstLetterUpperCase( aportanteEt.cofinanciacionAportante.municipio.descripcion ) }`;
                    }

                    listaNombreAportante.push(
                        {
                            tipoAportanteId: aportanteEt.cofinanciacionAportante.tipoAportanteId,
                            cofinanciacionAportanteId: aportanteEt.cofinanciacionAportanteId,
                            nombreAportante
                        }
                    )
                }

                tieneAportanteEt = true;
            } else {
                tieneAportanteEt = false;
            }

            // verificar si en la lista de aportante, tiene aportante de tipo Tercero
            const aportanteTercero = aportantes.find( aportante => aportante.cofinanciacionAportante.tipoAportanteId === this.listaTipoAportantes.tercero );
            if ( aportanteTercero !== undefined ) {
                const tieneAportante = listaNombreAportante.includes( aportanteTercero.cofinanciacionAportanteId );

                if ( tieneAportante === false ) {
                    listaNombreAportante.push(
                        {
                            tipoAportanteId: aportanteTercero.cofinanciacionAportante.tipoAportanteId,
                            cofinanciacionAportanteId: aportanteTercero.cofinanciacionAportanteId,
                            nombreAportante: `${ this.firstLetterUpperCase( aportanteTercero.cofinanciacionAportante.nombreAportante.nombre ) }`
                        }
                    )
                }

                tieneAportanteTercero = true;
            } else {
                tieneAportanteTercero = false;
            }

            const listaTipoAportante = await this.commonSvc.listaTipoAportante().toPromise()
            if ( tieneAportanteFfie === false ) {
                const indexAportante = listaTipoAportante.findIndex( aportante => aportante.dominioId === this.listaTipoAportantes.ffie );

                if ( indexAportante !== -1 ) {
                    listaTipoAportante.splice( indexAportante, 1 );
                }
            }

            if ( tieneAportanteEt === false ) {
                const indexAportante = listaTipoAportante.findIndex( aportante => aportante.dominioId === this.listaTipoAportantes.et );

                if ( indexAportante !== -1 ) {
                    listaTipoAportante.splice( indexAportante, 1 );
                }
            }

            if ( tieneAportanteTercero === false ) {
                const indexAportante = listaTipoAportante.findIndex( aportante => aportante.dominioId === this.listaTipoAportantes.tercero );

                if ( indexAportante !== -1 ) {
                    listaTipoAportante.splice( indexAportante, 1 );
                }
            }

            return new Promise<{ listaTipoAportante, listaNombreAportante }>( resolve => resolve ( { listaTipoAportante, listaNombreAportante } ) );
        }
    }

    async getAportantesNew( solicitudPago: any ) {
      if ( solicitudPago !== undefined ) {
          // constantes y variables
          const tablaInformacionFuenteRecursos: any[] = solicitudPago.tablaInformacionFuenteRecursos;
          const aportantes: any[] = [];
          const listaNombreAportante: { tipoAportanteId: number, cofinanciacionAportanteId: number, nombreAportante: string }[] = [];
          const listaTipoAportanteTmp = await this.commonSvc.listaTipoAportante().toPromise();
          const listaTipoAportante = [];
          // Get lista de aportantes
          for ( const aportante of tablaInformacionFuenteRecursos ) {
            aportantes.push( aportante );
            if(!listaNombreAportante.find(r => r.cofinanciacionAportanteId == aportante.cofinanciacionAportanteId)){
              listaNombreAportante.push(
                {
                    tipoAportanteId: aportante.tipoAportanteId,
                    cofinanciacionAportanteId: aportante.cofinanciacionAportanteId,
                    nombreAportante: aportante.nombreAportante
                }
              );
            }

            listaTipoAportante.push(listaTipoAportanteTmp.find(r => r.dominioId == aportante.tipoAportanteId));
          }

          return new Promise<{ listaTipoAportante, listaNombreAportante }>( resolve => resolve ( { listaTipoAportante, listaNombreAportante } ) );
      }
  }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

}
