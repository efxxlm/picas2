import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { DevolverActa } from '../../../_interfaces/compromisos-actas-comite.interfaces';
import { Respuesta } from '../autenticacion/autenticacion.service';

@Injectable({
  providedIn: 'root'
})
export class CompromisosActasComiteService {

  private url: string = `${ environment.apiUrl }/ManagementCommitteeReport`;
  devolverActa: DevolverActa = {
    comiteTecnicoId: 0,
    observacion: '',
    fecha: null,
    sesionComentarioId: null
  }

  constructor ( private http: HttpClient ) {};

  getGrillaCompromisos () {
    return this.http.get( `${ this.url }/GetListCompromisos` )
  };

  getCompromiso ( compromisoId: number, tipoCompromiso: number ) {
    return this.http.get( `${ this.url }/GetListCompromisoSeguimiento?SesionSolicitudCompromisoId=${ compromisoId }&pTipoCompromiso=${ tipoCompromiso }` )
  };

  guardarObservacionStorage ( observacion: string, sesionComiteTecnicoCompromisoId: number ) {
    const observacionGestion = [];
    if ( localStorage.getItem( 'observacionGestion' ) ) {
      observacionGestion.push( {
        observacion, 
        sesionComiteTecnicoCompromisoId
      } )
    } else {
      observacionGestion.push({
        observacion, 
        sesionComiteTecnicoCompromisoId
      })
    };
    localStorage.setItem( 'observacionGestion', JSON.stringify( observacionGestion ) );
  };

  cargarObservacionGestion ( sesionComiteTecnicoCompromisoId: number ) {
    if ( localStorage.getItem( 'observacionGestion' ) ) {
      let observacion;
      let observacionGestion: any[] = JSON.parse( localStorage.getItem( 'observacionGestion' ) );
      observacionGestion.forEach( value => {
        if ( value.sesionComiteTecnicoCompromisoId === sesionComiteTecnicoCompromisoId ) {
          observacion = value.observacion;
        };
      } );
      return new Promise( resolve => {
        if ( observacion ) {
          resolve( observacion );
        } else {
          resolve( null );
        }
      } );
    }
  }

  eliminarObservacionStorage ( sesionComiteTecnicoCompromisoId: number ) {
    if ( localStorage.getItem( 'observacionGestion' ) ) {
      let observacionGestion: any[] = JSON.parse( localStorage.getItem( 'observacionGestion' ) );
      if ( observacionGestion.length > 0 ) {
        observacionGestion.forEach( ( value, index ) => {
          if ( value.sesionComiteTecnicoCompromisoId === sesionComiteTecnicoCompromisoId ) {
            observacionGestion.splice( index, 1 );
          };
        } );
        localStorage.setItem( 'observacionGestion', JSON.stringify( observacionGestion ) );
      }
    }
  };

  getGrillaActas () {
    return this.http.get ( `${ this.url }/GetManagementReport` )
  };

  getActa ( comiteTecnicoId: number ) {
    return this.http.get( `${ this.url }/GetManagementReportById?comiteTecnicoId=${ comiteTecnicoId }` )
  }

  aprobarActa ( comiteTecnicoId: number ) {
    return this.http.post( `${ this.url }/AcceptReport?comiteTecnicoId=${ comiteTecnicoId }`, '' );
  }

  postCompromisos ( comite: any, estadoId: string ) {
    const gestionRealizada = comite.tarea;

    const pSesionSolicitudCompromiso = {
      sesionSolicitudCompromisoId: comite.compromisoId,
      tipoCompromiso: comite.tipoSolicitud,
      estadoCodigo: estadoId,
      gestionRealizada
    };

    return this.http.post<Respuesta>( `${ this.url }/ChangeStatusSesionComiteSolicitudCompromiso`, pSesionSolicitudCompromiso )
  };

  getSelectionProcessById ( id : number ) {
    return this.http.get( `${ environment.apiUrl }/SelectionProcess/GetSelectionProcessById?id=${ id }` )
  }

  postComentariosActa ( acta: any ) {
    this.devolverActa.comiteTecnicoId = acta.comiteTecnicoId;
    this.devolverActa.observacion = acta.observaciones;

    return this.http.post( `${ this.url }/CreateOrEditCommentReport`, this.devolverActa )
  };

};