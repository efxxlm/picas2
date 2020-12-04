import { DetalleDialogPlanesProgramasComponent } from './../detalle-dialog-planes-programas/detalle-dialog-planes-programas.component';
import { Component, EventEmitter, Input, OnInit, Output,  } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { FaseDosAprobarConstruccionService } from 'src/app/core/_services/faseDosAprobarConstruccion/fase-dos-aprobar-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-planes-programas-artc',
  templateUrl: './planes-programas-artc.component.html',
  styleUrls: ['./planes-programas-artc.component.scss']
})
export class PlanesProgramasArtcComponent implements OnInit {


  dataPlanesProgramas: any[] = [];
  dataSource = new MatTableDataSource();
  dataTableHistorial = new MatTableDataSource();
  dataTableHistorialApoyo = new MatTableDataSource();
  displayedColumns: string[] = [
    'planesProgramas',
    'recibioRequisito',
    'fechaRadicado',
    'fechaAprobacion',
    'requiereObservacion',
    'observaciones'
  ];
  displayedColumnsHistorial: string[] = [
    'fechaRevision',
    'observacionesSupervision'
  ];
  booleanosRequisitos: any[] = [
    { value: true, viewValue: 'Si' },
    { value: false, viewValue: 'No' }
  ];
  require: any;
  dataTablaHistorialObservacion: any[] = [];
  dataTableApoyo: any[] = [];
  booleanosObservacion: any[] = [
    { value: true, viewValue: 'Si' },
    { value: false, viewValue: 'No' }
  ];
  urlSoporte: string;
  totalGuardados = 0;
  observacionPlanesProgramas = '2';
  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required],
    construccionObservacionId: []
  });
  editorStyle = {
    height: '100px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  @Input() observacionesCompleted: boolean;
  @Input() planesProgramas: any;
  @Input() contratoConstruccionId: any;
  @Output() createEdit = new EventEmitter();

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private faseDosAprobarConstruccionSvc: FaseDosAprobarConstruccionService )
  {
  }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataPlanesProgramas);
    this.getDataPlanesProgramas();
    this.getDataTablePlanesProgramas();
  }


  guardar() {
    console.log( this.dataPlanesProgramas );
    console.log( this.urlSoporte );
  }

  getDataTablePlanesProgramas() {
    this.planesProgramas.construccionObservacion.forEach( observacion => {
      if (  observacion.tipoObservacionConstruccion === this.observacionPlanesProgramas
            && observacion.observaciones !== undefined
            && observacion.esSupervision === true ) {
        this.dataTablaHistorialObservacion.push( observacion );
      }
      if (  observacion.tipoObservacionConstruccion === this.observacionPlanesProgramas
            && observacion.observaciones !== undefined
            && observacion.esSupervision === false ) {
        this.dataTableApoyo.push( observacion );
      }
    } );
    if ( this.dataTablaHistorialObservacion.length > 0 ) {
      this.dataTableHistorial = new MatTableDataSource( this.dataTablaHistorialObservacion );
    }
    if ( this.dataTableApoyo.length > 0 ) {
      this.dataTableHistorialApoyo = new MatTableDataSource( this.dataTableApoyo );
    }
  }

  getDataPlanesProgramas() {
    this.urlSoporte = this.planesProgramas.planRutaSoporte ? this.planesProgramas.planRutaSoporte : null;
    this.dataPlanesProgramas.push(
      {
        nombrePlanesProgramas: 'Licencia vigente',
        recibioRequisito: this.planesProgramas.planLicenciaVigente !== undefined ? this.planesProgramas.planLicenciaVigente : null,
        fechaRadicado: this.planesProgramas.licenciaFechaRadicado ? new Date(this.planesProgramas.licenciaFechaRadicado) : null,
        fechaAprobacion: this.planesProgramas.licenciaFechaAprobacion ? new Date(this.planesProgramas.licenciaFechaAprobacion) : null,
        requiereObservacion:  this.planesProgramas.licenciaConObservaciones !== undefined ?
                              this.planesProgramas.licenciaConObservaciones : null,
        observaciones: this.planesProgramas.licenciaObservaciones ? this.planesProgramas.licenciaObservaciones : null,
        id: 1
      },
      {
        nombrePlanesProgramas: 'Cambio constructor responsable de la licencia',
        recibioRequisito: this.planesProgramas.planCambioConstructorLicencia !== undefined ?
                          this.planesProgramas.planCambioConstructorLicencia : null,
        fechaRadicado: this.planesProgramas.cambioFechaRadicado ? new Date(this.planesProgramas.cambioFechaRadicado) : null,
        fechaAprobacion: this.planesProgramas.cambioFechaAprobacion ? new Date(this.planesProgramas.cambioFechaAprobacion) : null,
        requiereObservacion: this.planesProgramas.cambioConObservaciones !== undefined ? this.planesProgramas.cambioConObservaciones : null,
        observaciones: this.planesProgramas.cambioObservaciones ? this.planesProgramas.cambioObservaciones : null,
        id: 2
      },
      {
        nombrePlanesProgramas: 'Acta aceptación y apropiación diseños',
        recibioRequisito: this.planesProgramas.planActaApropiacion !== undefined ? this.planesProgramas.planActaApropiacion : null,
        fechaRadicado:  this.planesProgramas.actaApropiacionFechaRadicado ?
                        new Date(this.planesProgramas.actaApropiacionFechaRadicado) : null,
        fechaAprobacion:  this.planesProgramas.actaApropiacionFechaAprobacion ?
                          new Date(this.planesProgramas.actaApropiacionFechaAprobacion) : null,
        requiereObservacion:  this.planesProgramas.actaApropiacionConObservaciones !== undefined ?
                              this.planesProgramas.actaApropiacionConObservaciones : null,
        observaciones: this.planesProgramas.actaApropiacionObservaciones ? this.planesProgramas.actaApropiacionObservaciones : null,
        id: 3
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de residuos de construcción y demolición (RCD) aprobado?',
        recibioRequisito: this.planesProgramas.planResiduosDemolicion !== undefined ? this.planesProgramas.planResiduosDemolicion : null,
        fechaRadicado:  this.planesProgramas.residuosDemolicionFechaRadicado ?
                        new Date(this.planesProgramas.residuosDemolicionFechaRadicado) : null,
        fechaAprobacion:  this.planesProgramas.residuosDemolicionFechaAprobacion ?
                          new Date(this.planesProgramas.residuosDemolicionFechaAprobacion) : null,
        requiereObservacion:  this.planesProgramas.residuosDemolicionConObservaciones !== undefined ?
                              this.planesProgramas.residuosDemolicionConObservaciones : null,
        observaciones: this.planesProgramas.residuosDemolicionObservaciones ? this.planesProgramas.residuosDemolicionObservaciones : null,
        id: 4
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo de tránsito (PMT) aprobado?',
        recibioRequisito: this.planesProgramas.planManejoTransito !== undefined ? this.planesProgramas.planManejoTransito : null,
        fechaRadicado: this.planesProgramas.manejoTransitoFechaRadicado ? new Date(this.planesProgramas.manejoTransitoFechaRadicado) : null,
        fechaAprobacion:  this.planesProgramas.manejoTransitoFechaAprobacion ?
                          new Date(this.planesProgramas.manejoTransitoFechaAprobacion) : null,
        requiereObservacion:  this.planesProgramas.manejoTransitoConObservaciones1 !== undefined ?
                              this.planesProgramas.manejoTransitoConObservaciones1 : null,
        observaciones: this.planesProgramas.manejoTransitoObservaciones ? this.planesProgramas.manejoTransitoObservaciones : null,
        id: 5
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo ambiental aprobado?',
        recibioRequisito: this.planesProgramas.planManejoAmbiental !== undefined ? this.planesProgramas.planManejoAmbiental : null,
        fechaRadicado:  this.planesProgramas.manejoAmbientalFechaRadicado ?
                        new Date(this.planesProgramas.manejoAmbientalFechaRadicado) : null,
        fechaAprobacion:  this.planesProgramas.manejoAmbientalFechaAprobacion ?
                          new Date(this.planesProgramas.manejoAmbientalFechaAprobacion) : null,
        requiereObservacion:  this.planesProgramas.manejoAmbientalConObservaciones !== undefined ?
                              this.planesProgramas.manejoAmbientalConObservaciones : null,
        observaciones: this.planesProgramas.manejoAmbientalObservaciones ? this.planesProgramas.manejoAmbientalObservaciones : null,
        id: 6
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de aseguramiento de la calidad de obra aprobado?',
        recibioRequisito: this.planesProgramas.planAseguramientoCalidad !== undefined ?
                          this.planesProgramas.planAseguramientoCalidad : null,
        fechaRadicado:  this.planesProgramas.aseguramientoCalidadFechaRadicado ?
                        new Date(this.planesProgramas.aseguramientoCalidadFechaRadicado) : null,
        fechaAprobacion:  this.planesProgramas.aseguramientoCalidadFechaAprobacion ?
                          new Date(this.planesProgramas.aseguramientoCalidadFechaAprobacion) : null,
        requiereObservacion:  this.planesProgramas.aseguramientoCalidadConObservaciones !== undefined ?
                              this.planesProgramas.aseguramientoCalidadConObservaciones : null,
        observaciones:  this.planesProgramas.aseguramientoCalidadObservaciones ?
                        this.planesProgramas.aseguramientoCalidadObservaciones : null,
        id: 8
      },
      {
        nombrePlanesProgramas: '¿Cuenta con programa de Seguridad industrial aprobado?',
        recibioRequisito: this.planesProgramas.planProgramaSeguridad !== undefined ? this.planesProgramas.planProgramaSeguridad : null,
        fechaRadicado:  this.planesProgramas.programaSeguridadFechaRadicado ?
                        new Date(this.planesProgramas.programaSeguridadFechaRadicado) : null,
        fechaAprobacion:  this.planesProgramas.programaSeguridadFechaAprobacion ?
                          new Date(this.planesProgramas.programaSeguridadFechaAprobacion) : null,
        requiereObservacion:  this.planesProgramas.programaSeguridadConObservaciones !== undefined ?
                              this.planesProgramas.programaSeguridadConObservaciones : null,
        observaciones: this.planesProgramas.programaSeguridadObservaciones ? this.planesProgramas.programaSeguridadObservaciones : null,
        id: 9
      },
      {
        nombrePlanesProgramas: '¿Cuenta con programa de salud ocupacional aprobado?',
        recibioRequisito: this.planesProgramas.planProgramaSalud !== undefined ? this.planesProgramas.planProgramaSalud : null,
        fechaRadicado: this.planesProgramas.programaSaludFechaRadicado ? new Date(this.planesProgramas.programaSaludFechaRadicado) : null,
        fechaAprobacion:  this.planesProgramas.programaSaludFechaAprobacion ?
                          new Date(this.planesProgramas.programaSaludFechaAprobacion) : null,
        requiereObservacion:  this.planesProgramas.programaSaludConObservaciones !== undefined ?
                              this.planesProgramas.programaSaludConObservaciones : null,
        observaciones: this.planesProgramas.programaSaludObservaciones ? this.planesProgramas.programaSaludObservaciones : null,
        id: 10
      },
      {
        nombrePlanesProgramas: '¿Cuenta con un plan inventario arbóreo (talas) aprobado?',
        recibioRequisito: this.planesProgramas.planInventarioArboreo !== undefined ? this.planesProgramas.planInventarioArboreo : null,
        fechaRadicado:  this.planesProgramas.inventarioArboreoFechaRadicado ?
                        new Date(this.planesProgramas.inventarioArboreoFechaRadicado) : null,
        fechaAprobacion:  this.planesProgramas.inventarioArboreoFechaAprobacion ?
                          new Date(this.planesProgramas.inventarioArboreoFechaAprobacion) : null,
        requiereObservacion:  this.planesProgramas.inventarioArboreoConObservaciones !== undefined ?
                              this.planesProgramas.inventarioArboreoConObservaciones : null,
        observaciones: this.planesProgramas.inventarioArboreoObservaciones ? this.planesProgramas.inventarioArboreoObservaciones : null,
        id: 11
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de aprovechamiento forestal aprobado?',
        recibioRequisito: this.planesProgramas.planAprovechamientoForestal !== undefined ?
                          this.planesProgramas.planAprovechamientoForestal : null,
        fechaRadicado:  this.planesProgramas.aprovechamientoForestalApropiacionFechaRadicado ?
                        new Date(this.planesProgramas.aprovechamientoForestalApropiacionFechaRadicado) : null,
        fechaAprobacion:  this.planesProgramas.aprovechamientoForestalFechaAprobacion ?
                          new Date(this.planesProgramas.aprovechamientoForestalFechaAprobacion) : null,
        requiereObservacion:  this.planesProgramas.aprovechamientoForestalConObservaciones !== undefined ?
                              this.planesProgramas.aprovechamientoForestalConObservaciones : null,
        observaciones:  this.planesProgramas.aprovechamientoForestalObservaciones ?
                        this.planesProgramas.aprovechamientoForestalObservaciones : null,
        id: 12
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo de aguas lluvias aprobado?',
        recibioRequisito: this.planesProgramas.planManejoAguasLluvias !== undefined ? this.planesProgramas.planManejoAguasLluvias : null,
        fechaRadicado:  this.planesProgramas.manejoAguasLluviasFechaRadicado ?
                        new Date(this.planesProgramas.manejoAguasLluviasFechaRadicado) : null,
        fechaAprobacion:  this.planesProgramas.manejoAguasLluviasFechaAprobacion ?
                          new Date(this.planesProgramas.manejoAguasLluviasFechaAprobacion) : null,
        requiereObservacion:  this.planesProgramas.manejoAguasLluviasConObservaciones !== undefined ?
                              this.planesProgramas.manejoAguasLluviasConObservaciones : null,
        observaciones: this.planesProgramas.manejoAguasLluviasObservaciones ? this.planesProgramas.manejoAguasLluviasObservaciones : null,
        id: 13
      }
    );

    this.addressForm.get('tieneObservaciones')
      .setValue(  this.planesProgramas.tieneObservacionesPlanesProgramasSupervisor !== undefined ?
                  this.planesProgramas.tieneObservacionesPlanesProgramasSupervisor : null );
    this.addressForm.get('observaciones')
      .setValue(  this.planesProgramas.observacionPlanesProgramasSupervisor !== undefined ?
                  this.planesProgramas.observacionPlanesProgramasSupervisor.observaciones : null );
    this.addressForm.get('construccionObservacionId')
      .setValue(  this.planesProgramas.observacionPlanesProgramasSupervisor !== undefined ?
                  this.planesProgramas.observacionPlanesProgramasSupervisor.construccionObservacionId : null);

  }

  validarSemaforo() {

    this.planesProgramas.semaforoPlanes = 'sin-diligenciar';

    if (this.addressForm.value.tieneObservaciones === true || this.addressForm.value.tieneObservaciones === false) {
      this.planesProgramas.semaforoPlanes = 'completo';

      if (this.addressForm.value.tieneObservaciones === true && !this.addressForm.value.observaciones) {
        this.planesProgramas.semaforoPlanes = 'en-proceso';
      }
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    if ( texto !== undefined ) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length > 1000 ? 1000 : textolimpio.length;
    } else {
      return 0;
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  }

  openDialogObservacion(planPrograma: string, observacion: string, id: number) {
    const dialogObservacion = this.dialog.open(DetalleDialogPlanesProgramasComponent, {
      width: '60em',
      data: { planPrograma, observacion }
    });

    dialogObservacion.afterClosed().subscribe(resp => {
      this.dataPlanesProgramas.forEach(data => {
        if (data.id === id) {
          data.observaciones = resp.data;
          return;
        }
      });
    });
  }

  guardarPlanes() {

    const construccion = {
      contratoConstruccionId: this.contratoConstruccionId,
      tieneObservacionesPlanesProgramasSupervisor: this.addressForm.value.tieneObservaciones,
      construccionObservacion: [
        {
          construccionObservacionId: this.addressForm.value.construccionObservacionId,
          contratoConstruccionId: this.contratoConstruccionId,
          tipoObservacionConstruccion: TiposObservacionConstruccion.PlanesProgramas,
          esSupervision: true,
          esActa: false,
          observaciones: this.addressForm.value.observaciones
        }
      ]
    };

    console.log( construccion );

    if (  this.addressForm.value.tieneObservaciones === false
          && this.totalGuardados === 0
          && this.planesProgramas.tieneObservacionesPlanesProgramasApoyo === true ) {
      this.openDialog( '', '<b>Le recomendamos verificar su respuesta; tenga en cuenta que el apoyo a la supervisión si tuvo observaciones.</b>' );
      this.totalGuardados++;
      return;
    }
    if ( this.totalGuardados === 1 || this.addressForm.value.tieneObservaciones !== null ) {
      this.faseDosAprobarConstruccionSvc.createEditObservacionPlanesProgramasSupervisor( construccion )
        .subscribe(
          response => {
            this.openDialog( '', response.message );
            this.createEdit.emit( true );
          },
          err => this.openDialog( '', err.message )
        );
    }

  }

}
