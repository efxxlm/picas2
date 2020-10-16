import { Component, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-planes-programas',
  templateUrl: './planes-programas.component.html',
  styleUrls: ['./planes-programas.component.scss']
})
export class PlanesProgramasComponent implements OnInit {

  @Input() planesProgramas: any;
  @Output() planesProgramasForm = new EventEmitter();
  @ViewChild( MatSort, { static: true } ) sort : MatSort;
  dataPlanesProgramas: any[] = [];
  dataSource                 = new MatTableDataSource();
  displayedColumns: string[] = [ 
    'planesProgramas',
    'recibioRequisito',
    'fechaRadicado',
    'fechaAprobacion',
    'requiereObservacion',
    'observaciones'
  ];
  booleanosRequisitos: any[] = [
    { value: true, viewValue: 'Si' },
    { value: false, viewValue: 'No' }
  ]
  requisitosNoSeRequire: any[] = [
    { value: 2, viewValue: 'Si' },
    { value: 1, viewValue: 'No' },
    { value: 3, viewValue: 'No se requiere' }
  ]
  require: any;
  booleanosObservacion: any[] = [
    { value: true, viewValue: 'Si' },
    { value: false, viewValue: 'No' }
  ]
  urlSoporte: string;

  constructor ( private dialog: MatDialog ) {
  }

  ngOnInit(): void {
    if ( this.planesProgramas ) {
      this.getDataPlanesProgramas();
    } else {
      this.getDataPlanes();
    }
    this.dataSource = new MatTableDataSource( this.dataPlanesProgramas );
    this.dataSource.sort                   = this.sort;
  }

  openDialog ( planPrograma: string, observacion: string, id: number ) {
    const dialogObservacion = this.dialog.open(DialogObservacionesComponent, {
      width: '60em',
      data : { planPrograma, observacion }
    });

    dialogObservacion.afterClosed().subscribe( resp => {
      this.dataPlanesProgramas.forEach( data => {
        if ( data.id === id ) {
          data.observaciones = resp.data;
          return;
        }
      } )
    } );
  };

  getSemaforo ( observacion: string ) {
    if ( observacion !== null ) {
      return 'completo';
    } else {
      return 'sin-diligenciar';
    }
  }

  guardar () {
    const planesProgramas = {
      dataPlanesProgramas: this.dataPlanesProgramas,
      urlSoporte: this.urlSoporte
    }
    this.planesProgramasForm.emit( planesProgramas );
  }

  getDataPlanesProgramas () {
    this.urlSoporte = this.planesProgramas.planRutaSoporte ? this.planesProgramas.planRutaSoporte : null;
    this.dataPlanesProgramas.push(
      {
        nombrePlanesProgramas: 'Licencia vigente',
        recibioRequisito: this.planesProgramas.planLicenciaVigente !== undefined ? this.planesProgramas.planLicenciaVigente : null,
        fechaRadicado: this.planesProgramas.licenciaFechaRadicado ? new Date( this.planesProgramas.licenciaFechaRadicado ) : null,
        fechaAprobacion: this.planesProgramas.licenciaFechaAprobacion ? new Date( this.planesProgramas.licenciaFechaAprobacion ) : null,
        requiereObservacion: this.planesProgramas.licenciaConObservaciones !== undefined ? this.planesProgramas.licenciaConObservaciones : null,
        observaciones: this.planesProgramas.licenciaObservaciones ? this.planesProgramas.licenciaObservaciones : null,
        id: 1
      },
      {
        nombrePlanesProgramas: 'Cambio constructor responsable de la licencia',
        recibioRequisito: this.planesProgramas.planCambioConstructorLicencia !== undefined ? this.planesProgramas.planCambioConstructorLicencia : null,
        fechaRadicado: this.planesProgramas.cambioFechaRadicado ? new Date( this.planesProgramas.cambioFechaRadicado ) : null,
        fechaAprobacion: this.planesProgramas.cambioFechaAprobacion ? new Date( this.planesProgramas.cambioFechaAprobacion ) : null,
        requiereObservacion: this.planesProgramas.cambioConObservaciones !== undefined ? this.planesProgramas.cambioConObservaciones : null,
        observaciones: this.planesProgramas.cambioObservaciones ? this.planesProgramas.cambioObservaciones : null,
        id: 2
      },
      {
        nombrePlanesProgramas: 'Acta aceptación y apropiación diseños',
        recibioRequisito: this.planesProgramas.planActaApropiacion !== undefined ? this.planesProgramas.planActaApropiacion : null,
        fechaRadicado: this.planesProgramas.actaApropiacionFechaRadicado ? new Date( this.planesProgramas.actaApropiacionFechaRadicado ) : null,
        fechaAprobacion: this.planesProgramas.actaApropiacionFechaAprobacion ? new Date( this.planesProgramas.actaApropiacionFechaAprobacion ) : null,
        requiereObservacion: this.planesProgramas.actaApropiacionConObservaciones !== undefined ? this.planesProgramas.actaApropiacionConObservaciones : null,
        observaciones: this.planesProgramas.actaApropiacionObservaciones ? this.planesProgramas.actaApropiacionObservaciones : null,
        id: 3
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de residuos de construcción y demolición (RCD) aprobado?',
        recibioRequisito: this.planesProgramas.planResiduosDemolicion !== undefined ? this.planesProgramas.planResiduosDemolicion : null,
        fechaRadicado: this.planesProgramas.residuosDemolicionFechaRadicado ? new Date( this.planesProgramas.residuosDemolicionFechaRadicado ) : null,
        fechaAprobacion: this.planesProgramas.residuosDemolicionFechaAprobacion ? new Date( this.planesProgramas.residuosDemolicionFechaAprobacion ) : null,
        requiereObservacion: this.planesProgramas.residuosDemolicionConObservaciones !== undefined ? this.planesProgramas.residuosDemolicionConObservaciones : null,
        observaciones: this.planesProgramas.residuosDemolicionObservaciones ? this.planesProgramas.residuosDemolicionObservaciones : null,
        id: 4
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo de tránsito (PMT) aprobado?',
        recibioRequisito: this.planesProgramas.planManejoTransito !== undefined ? this.planesProgramas.planManejoTransito : null,
        fechaRadicado: this.planesProgramas.manejoTransitoFechaRadicado ? new Date( this.planesProgramas.manejoTransitoFechaRadicado ) : null,
        fechaAprobacion: this.planesProgramas.manejoTransitoFechaAprobacion ? new Date( this.planesProgramas.manejoTransitoFechaAprobacion ) : null,
        requiereObservacion: this.planesProgramas.manejoTransitoConObservaciones1 !== undefined ? this.planesProgramas.manejoTransitoConObservaciones1 : null,
        observaciones: this.planesProgramas.manejoTransitoObservaciones ? this.planesProgramas.manejoTransitoObservaciones : null,
        id: 5
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo ambiental aprobado?',
        recibioRequisito: this.planesProgramas.planManejoAmbiental !== undefined ? this.planesProgramas.planManejoAmbiental : null,
        fechaRadicado: this.planesProgramas.manejoAmbientalFechaRadicado ? new Date( this.planesProgramas.manejoAmbientalFechaRadicado ) : null,
        fechaAprobacion: this.planesProgramas.manejoAmbientalFechaAprobacion ? new Date( this.planesProgramas.manejoAmbientalFechaAprobacion ) : null,
        requiereObservacion: this.planesProgramas.manejoAmbientalConObservaciones !== undefined ? this.planesProgramas.manejoAmbientalConObservaciones : null,
        observaciones: this.planesProgramas.manejoAmbientalObservaciones ? this.planesProgramas.manejoAmbientalObservaciones : null,
        id: 6
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de aseguramiento de la calidad de obra aprobado?',
        recibioRequisito: this.planesProgramas.planAseguramientoCalidad !== undefined ? this.planesProgramas.planAseguramientoCalidad : null,
        fechaRadicado: this.planesProgramas.aseguramientoCalidadFechaRadicado ? new Date( this.planesProgramas.aseguramientoCalidadFechaRadicado ) : null,
        fechaAprobacion: this.planesProgramas.aseguramientoCalidadFechaAprobacion ? new Date( this.planesProgramas.aseguramientoCalidadFechaAprobacion ) : null,
        requiereObservacion: this.planesProgramas.aseguramientoCalidadConObservaciones !== undefined ? this.planesProgramas.aseguramientoCalidadConObservaciones : null,
        observaciones: this.planesProgramas.aseguramientoCalidadObservaciones ? this.planesProgramas.aseguramientoCalidadObservaciones : null,
        id: 8
      },
      {
        nombrePlanesProgramas: '¿Cuenta con programa de Seguridad industrial aprobado?',
        recibioRequisito: this.planesProgramas.planProgramaSeguridad !== undefined ? this.planesProgramas.planProgramaSeguridad : null,
        fechaRadicado: this.planesProgramas.programaSeguridadFechaRadicado ? new Date( this.planesProgramas.programaSeguridadFechaRadicado ) : null,
        fechaAprobacion: this.planesProgramas.programaSeguridadFechaAprobacion ? new Date( this.planesProgramas.programaSeguridadFechaAprobacion ) : null,
        requiereObservacion: this.planesProgramas.programaSeguridadConObservaciones !== undefined ? this.planesProgramas.programaSeguridadConObservaciones : null,
        observaciones: this.planesProgramas.programaSeguridadObservaciones ? this.planesProgramas.programaSeguridadObservaciones : null,
        id: 9
      },
      {
        nombrePlanesProgramas: '¿Cuenta con programa de salud ocupacional aprobado?',
        recibioRequisito: this.planesProgramas.planProgramaSalud !== undefined ? this.planesProgramas.planProgramaSalud : null,
        fechaRadicado: this.planesProgramas.programaSaludFechaRadicado ? new Date( this.planesProgramas.programaSaludFechaRadicado ) : null,
        fechaAprobacion: this.planesProgramas.programaSaludFechaAprobacion ? new Date( this.planesProgramas.programaSaludFechaAprobacion ) : null,
        requiereObservacion: this.planesProgramas.programaSaludConObservaciones !== undefined ? this.planesProgramas.programaSaludConObservaciones : null,
        observaciones: this.planesProgramas.programaSaludObservaciones ? this.planesProgramas.programaSaludObservaciones : null,
        id: 10
      },
      {
        nombrePlanesProgramas: '¿Cuenta con un plan inventario arbóreo (talas) aprobado?',
        recibioRequisito: this.planesProgramas.planInventarioArboreo !== undefined ? this.planesProgramas.planInventarioArboreo : null,
        fechaRadicado: this.planesProgramas.inventarioArboreoFechaRadicado ? new Date( this.planesProgramas.inventarioArboreoFechaRadicado ) : null,
        fechaAprobacion: this.planesProgramas.inventarioArboreoFechaAprobacion ? new Date( this.planesProgramas.inventarioArboreoFechaAprobacion ) : null,
        requiereObservacion: this.planesProgramas.inventarioArboreoConObservaciones !== undefined ? this.planesProgramas.inventarioArboreoConObservaciones : null,
        observaciones: this.planesProgramas.inventarioArboreoObservaciones ? this.planesProgramas.inventarioArboreoObservaciones : null,
        id: 11
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de aprovechamiento forestal aprobado?',
        recibioRequisito: this.planesProgramas.planAprovechamientoForestal !== undefined ? this.planesProgramas.planAprovechamientoForestal : null,
        fechaRadicado: this.planesProgramas.aprovechamientoForestalApropiacionFechaRadicado ? new Date( this.planesProgramas.aprovechamientoForestalApropiacionFechaRadicado ) : null,
        fechaAprobacion: this.planesProgramas.aprovechamientoForestalFechaAprobacion ? new Date( this.planesProgramas.aprovechamientoForestalFechaAprobacion ) : null,
        requiereObservacion: this.planesProgramas.aprovechamientoForestalConObservaciones !== undefined ? this.planesProgramas.aprovechamientoForestalConObservaciones : null,
        observaciones: this.planesProgramas.aprovechamientoForestalObservaciones ? this.planesProgramas.aprovechamientoForestalObservaciones : null,
        id: 12
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo de aguas lluvias aprobado?',
        recibioRequisito: this.planesProgramas.planManejoAguasLluvias !== undefined ? this.planesProgramas.planManejoAguasLluvias : null,
        fechaRadicado: this.planesProgramas.manejoAguasLluviasFechaRadicado ? new Date( this.planesProgramas.manejoAguasLluviasFechaRadicado ) : null,
        fechaAprobacion: this.planesProgramas.manejoAguasLluviasFechaAprobacion ? new Date( this.planesProgramas.manejoAguasLluviasFechaAprobacion ) : null,
        requiereObservacion: this.planesProgramas.manejoAguasLluviasConObservaciones !== undefined ? this.planesProgramas.manejoAguasLluviasConObservaciones : null,
        observaciones: this.planesProgramas.manejoAguasLluviasObservaciones ? this.planesProgramas.manejoAguasLluviasObservaciones : null,
        id: 13
      }
    );
  };

  getDataPlanes () {
    this.urlSoporte = null;
    this.dataPlanesProgramas.push(
      {
        nombrePlanesProgramas: 'Licencia vigente',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 1
      },
      {
        nombrePlanesProgramas: 'Cambio constructor responsable de la licencia',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 2
      },
      {
        nombrePlanesProgramas: 'Acta aceptación y apropiación diseños',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 3
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de residuos de construcción y demolición (RCD) aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 4
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo de tránsito (PMT) aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 5
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo ambiental aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 6
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de aseguramiento de la calidad de obra aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 8
      },
      {
        nombrePlanesProgramas: '¿Cuenta con programa de Seguridad industrial aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 9
      },
      {
        nombrePlanesProgramas: '¿Cuenta con programa de salud ocupacional aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 10
      },
      {
        nombrePlanesProgramas: '¿Cuenta con un plan inventario arbóreo (talas) aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 11
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de aprovechamiento forestal aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 12
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo de aguas lluvias aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 13
      }
    );
  };

};