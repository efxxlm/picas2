import { Component, OnInit } from '@angular/core';
import { ProcesoSeleccion, ProcesoSeleccionService, TiposProcesoSeleccion, EstadosProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { forkJoin } from 'rxjs';
import { timeout } from 'rxjs/operators';

@Component({
  selector: 'app-invitacion-cerrada',
  templateUrl: './invitacion-cerrada.component.html',
  styleUrls: ['./invitacion-cerrada.component.scss']
})
export class InvitacionCerradaComponent implements OnInit {

  /*con este bit controlo los botones, esto lo hago ya sea por el estado del proyecto o en un futuro por el 
    permiso que tenga el usuario
    */
  bitPuedoEditar = true;

  tiposProcesoSeleccion = TiposProcesoSeleccion;
  estadosProcesoSeleccion = EstadosProcesoSeleccion;

  procesoSeleccion: ProcesoSeleccion = {
    alcanceParticular: '',
    criteriosSeleccion: '',
    esDistribucionGrupos: false,
    justificacion: '',
    numeroProceso: '',
    objeto: '',
    procesoSeleccionId: 0,
    tipoAlcanceCodigo: '',
    tipoIntervencionCodigo: '',
    tipoProcesoCodigo: '',
    procesoSeleccionGrupo: [],
    procesoSeleccionCronograma: [],
    estadoProcesoSeleccionCodigo: this.estadosProcesoSeleccion.Creado,


  };
  descripcion_class: number = 0;
  estudio_class: number = 0;
  datos_class: number = 0;
  evaluacion_class: number = 3;//asumo que en creación no tiene el estado necesario
  proponentes_class: number = 3;//asumo que en creación no tiene el estado necesario

  constructor(
    private procesoSeleccionService: ProcesoSeleccionService,
    private dialog: MatDialog,
    private router: Router,
    private activatedRoute: ActivatedRoute

  ) { }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe(async parametro => {

      this.procesoSeleccion.procesoSeleccionId = parametro['id'];
      this.procesoSeleccion.tipoProcesoCodigo = this.tiposProcesoSeleccion.Cerrada;

      if (this.procesoSeleccion.procesoSeleccionId > 0)
        this.editMode();

    })

  }

  openDialog(modalTitle: string, modalText: string, id: number) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.router.navigate(["/seleccion/invitacionCerrada", id]);
      setTimeout(() => {
        location.reload();
      }, 1000);
    });
  }

  async cargarRegistro() {

    return new Promise(resolve => {

      forkJoin([
        this.procesoSeleccionService.getProcesoSeleccionById(this.procesoSeleccion.procesoSeleccionId)
      ]).subscribe(proceso => {
        this.procesoSeleccion = proceso[0];
        this.descripcion_class = this.estaIncompletoDescripcion(this.procesoSeleccion);
        this.estudio_class = this.estaIncompletoEstudio(this.procesoSeleccion);
        this.datos_class = this.estaIncompletoDatos(this.procesoSeleccion);
        this.evaluacion_class = this.estaIncompletoEvaluacion(this.procesoSeleccion);
        this.proponentes_class = this.estaIncompletoProponentes(this.procesoSeleccion);
        setTimeout(() => { resolve(); }, 1000)
      });
    });

  }

  async editMode() {


    this.cargarRegistro().then(() => {

      let botonDescripcion = document.getElementById('botonDescripcion');
      let botonEstudio = document.getElementById('botonEstudio')
      let botonProponente = document.getElementById('botonProponente')
      let botonevaluacion = document.getElementById('botonevaluacion')
      let botonProponenteInvitar = document.getElementById('botonProponenteInvitar')


      botonDescripcion.click();
      botonEstudio.click();
      botonProponente.click();
      botonevaluacion.click();
      botonProponenteInvitar.click();

      console.log(this.procesoSeleccion.estadoProcesoSeleccionCodigo, this.estadosProcesoSeleccion.AprobadaSelecciónPorComiteFiduciario)

      //confirmo si tiene el estado para editar
      if (
        this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.Creado ||
        this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario ||
        this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.DevueltaAperturaPorComiteFiduciario ||
        this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.DevueltaAperturaPorComiteTecnico ||
        this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.DevueltaSeleccionPorComiteFiduciario ||
        this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.DevueltaSeleccionPorComiteTecnico ||
        this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.DevueltoPorComiteFiduciario ||
        this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.DevueltoPorComiteTecnico ||
        this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.AprobadaSelecciónPorComiteFiduciario
      ) {
        this.bitPuedoEditar = true;
      }
      else {
        this.bitPuedoEditar = false;
      }

    });

  }

  onSubmit() {
    this.procesoSeleccionService.guardarEditarProcesoSeleccion(this.procesoSeleccion).subscribe(respuesta => {
      // if ( respuesta.code == "200" )
      // {
      //   this.router.navigate([`/seleccion/seccionPrivada/${ this.procesoSeleccion.tipoProcesoCodigo }/${ this.procesoSeleccion.procesoSeleccionId }`])
      // }
      this.openDialog("", respuesta.message, respuesta.data.procesoSeleccionId)

      console.log('respuesta', respuesta);
    })
  }

  getStyleEvaluacion() {
    if (this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario)
      return 'auto'
    else
      return 'none'

  }

  getStyleProponentesSeleccionados() {
    if (this.procesoSeleccion.evaluacionDescripcion && this.procesoSeleccion.evaluacionDescripcion.length > 0)
      return 'auto'
    else
      return 'none'
  }

  getStyleProponentesCreado() {
    if (this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.Creado)
      return 'auto'
    else
      return 'none'
  }

  estaIncompletoDescripcion(pProceso: any): number {


    console.error(pProceso);

    let retorno: number = 0; //sin-diligenciar

    // verifico si hay algo registrado
    if (
      (pProceso.objeto !== "" && pProceso.objeto !== undefined) ||
      (pProceso.alcanceParticular !== "" && pProceso.alcanceParticular !== undefined) ||
      (pProceso.justificacion !== "" && pProceso.justificacion !== undefined) ||
      (pProceso.criteriosSeleccion !== "" && pProceso.criteriosSeleccion !== undefined) ||
      (pProceso.tipoIntervencionCodigo !== "" && pProceso.tipoIntervencionCodigo !== undefined) ||
      (pProceso.tipoAlcanceCodigo !== "" && pProceso.tipoAlcanceCodigo !== undefined) ||
      (pProceso.esDistribucionGrupos !== undefined) ||
      (pProceso.tipoResponsableTecnicoCodigo !== "" && pProceso.tipoResponsableTecnicoCodigo !== undefined) ||
      (pProceso.tipoResponsableEstructuradorCodigo !== "" && pProceso.tipoResponsableEstructuradorCodigo !== undefined) ||
      //(pProceso.condicionesAsignacionPuntaje !== undefined && pProceso.condicionesAsignacionPuntaje !== '' ) ||
      //(pProceso.condicionesFinancierasHabilitantes !== undefined && pProceso.condicionesFinancierasHabilitantes !== '' ) ||
      //(pProceso.condicionesJuridicasHabilitantes !== undefined && pProceso.condicionesJuridicasHabilitantes !== '' ) ||
      //(pProceso.condicionesTecnicasHabilitantes !== undefined && pProceso.condicionesTecnicasHabilitantes !== '' ) ||
      (pProceso.procesoSeleccionGrupo !== undefined && pProceso.procesoSeleccionGrupo.length > 0) ||
      (pProceso.procesoSeleccionCronograma !== undefined && pProceso.procesoSeleccionCronograma.length > 0)) {

      retorno = 2; // completo

      //Verifico si falta algo
      if (
        (pProceso.objeto === "" || pProceso.objeto === undefined) ||
        (pProceso.alcanceParticular === "" || pProceso.alcanceParticular === undefined) ||
        (pProceso.justificacion === "" || pProceso.justificacion === undefined) ||
        (pProceso.criteriosSeleccion === "" || pProceso.criteriosSeleccion === undefined) ||
        (pProceso.tipoIntervencionCodigo === "" || pProceso.tipoIntervencionCodigo === undefined) ||
        (pProceso.tipoAlcanceCodigo === "" || pProceso.tipoAlcanceCodigo === undefined) ||
        (pProceso.esDistribucionGrupos === undefined) ||
        
        (pProceso.tipoResponsableTecnicoCodigo == undefined && pProceso.tipoResponsableTecnicoCodigo === "") ||
        (pProceso.tipoResponsableEstructuradorCodigo == undefined && pProceso.tipoResponsableEstructuradorCodigo === "") ||
        //(pProceso.condicionesAsignacionPuntaje === undefined || pProceso.condicionesAsignacionPuntaje === '' ) ||
        //(pProceso.condicionesFinancierasHabilitantes === undefined || pProceso.condicionesFinancierasHabilitantes === '' ) ||
        //(pProceso.condicionesJuridicasHabilitantes === undefined || pProceso.condicionesJuridicasHabilitantes === '' ) ||
        //(pProceso.condicionesTecnicasHabilitantes === undefined || pProceso.condicionesTecnicasHabilitantes === '' ) ||
        (pProceso.procesoSeleccionGrupo === undefined || pProceso.procesoSeleccionGrupo.length === 0) ||
        (pProceso.procesoSeleccionCronograma === undefined || pProceso.procesoSeleccionCronograma.length === 0)
      ) {

        retorno = 1; // en-proceso  

      }

      // grupos
      pProceso.procesoSeleccionGrupo.forEach(psg => {
        if (
          psg.nombreGrupo === undefined ||
          psg.nombreGrupo === '' ||
          psg.tipoPresupuestoCodigo === undefined ||
          (psg.tipoPresupuestoCodigo === "2" && psg.valorMaximoCategoria === undefined) ||
          (psg.tipoPresupuestoCodigo === "2" && psg.valorMinimoCategoria === undefined) ||
          (psg.tipoPresupuestoCodigo === "1" && psg.valor === undefined) ||
          psg.plazoMeses === undefined
        ) {
          retorno = 1; // en-proceso   
        }
      });

    }
    return retorno;
  }

  estaIncompletoDatos(pProceso: any): number {
    let retorno = 0;
    // console.log("vantidad propo" + pProceso.procesoSeleccionProponente.length);

    if (pProceso.registroCompletoProponentes === false) {
      retorno = 2;
    }
    else if (pProceso.registroCompletoProponentes === true) {
      retorno = 1;
    }

    return retorno;
  }

  estaIncompletoEvaluacion(pProceso: any): number {
    let retorno = 0;
    console.log(pProceso.estadoProcesoSeleccionCodigo, EstadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario)
    if (
      pProceso.estadoProcesoSeleccionCodigo == EstadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario ||
      pProceso.estadoProcesoSeleccionCodigo == EstadosProcesoSeleccion.AprobadaSelecciónPorComiteFiduciario
    ) {
      if (pProceso.evaluacionDescripcion) {
        if (pProceso.evaluacionDescripcion != "" || pProceso.urlSoporteEvaluacion != "") {
          if (pProceso.evaluacionDescripcion != undefined && pProceso.evaluacionDescripcion != "" && pProceso.urlSoporteEvaluacion != "" && pProceso.urlSoporteEvaluacion != undefined) {
            retorno = 2;
          }
          else {
            retorno = 1;
          }
        }
      }

    }
    else {
      retorno = 3;
    }
    return retorno;
  }
  estaIncompletoProponentes(pProceso: any): number {
    console.log(pProceso)
    let retorno = 0;
    if (
      pProceso.estadoProcesoSeleccionCodigo != EstadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario &&
      pProceso.estadoProcesoSeleccionCodigo != EstadosProcesoSeleccion.AprobadaSelecciónPorComiteFiduciario
    ) {
      retorno = 3;
    }
    else if
      (
      pProceso.cantidadProponentes > 0 &&
      pProceso.urlSoporteProponentesSeleccionados !== '' &&
      pProceso.urlSoporteProponentesSeleccionados !== undefined
    ) {
      return 2;
    }else if ( 
      pProceso.cantidadProponentes > 0 ||
      (pProceso.urlSoporteProponentesSeleccionados !== '' &&
      pProceso.urlSoporteProponentesSeleccionados !== undefined)
     ){
      return 1;
    }

    return retorno;
  }


  estaIncompletoEstudio(pProceso: any): number {
    // 0 sin diligenciar
    // 1 en proceso
    // 2 completo

    let retorno = 0;

    if (pProceso.cantidadCotizaciones || pProceso.procesoSeleccionCotizacion.length > 0) {
      if (pProceso.cantidadCotizaciones && pProceso.procesoSeleccionCotizacion.length > 0) {
        retorno = 2;
        pProceso.procesoSeleccionCotizacion.forEach(cot => {
          if (
            cot.nombreOrganizacion === undefined ||
            cot.valorCotizacion === undefined ||
            cot.descripcion === undefined ||
            cot.urlSoporte === undefined
          )
            retorno = 1;
        });

      }
      else {
        retorno = 1;
      }
    }


    return retorno;
  }

  mostrarAyudaProcesoAprobado() {
    if (
      this.procesoSeleccion.estadoProcesoSeleccionCodigo != this.estadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario &&
      this.procesoSeleccion.estadoProcesoSeleccionCodigo != this.estadosProcesoSeleccion.AprobadaSelecciónPorComiteFiduciario
    ) {
      return true;
    }
    else {
      return false;
    }
  }

}
