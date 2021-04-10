import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import {
  ProcesoSeleccionService,
  ProcesoSeleccionCronograma,
  ProcesoSeleccionMonitoreo,
  ProcesoSeleccionCronogramaMonitoreo,
  EstadosProcesoSeleccion,
  EstadosProcesoSeleccionMonitoreo
} from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { ActivatedRoute } from '@angular/router';
import { forkJoin, from } from 'rxjs';
import { mergeMap, tap, toArray } from 'rxjs/operators';
import { CommonService, Dominio, Respuesta } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DialogDevolucionComponent } from '../dialog-devolucion/dialog-devolucion.component';
import { MatDialog } from '@angular/material/dialog';
import { EstadosSolicitud, EstadosSolicitudCronograma } from 'src/app/_interfaces/project-contracting';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
export interface ProcesosElement {
  id: any;
  tipo: string;
  numero: string;
  fechaSolicitud: string;
  numeroSolicitud: string;
  estadoDelSolicitud: string;
}
@Component({
  selector: 'app-tabla-cronograma',
  templateUrl: './tabla-cronograma.component.html',
  styleUrls: ['./tabla-cronograma.component.scss']
})
export class TablaCronogramaComponent implements OnInit {
  displayedColumns: string[] = ['tipo', 'numero', 'fechaSolicitud', 'numeroSolicitud', 'estadoDelSolicitud', 'id'];
  dataSource = new MatTableDataSource();
  procesoSeleccionMonitoreoId = 0;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  @Input() editMode: any = {};

  estadosProcesoSeleccion = EstadosProcesoSeleccion;
  estadosProcesoSeleccionMonitoreo = EstadosProcesoSeleccionMonitoreo;

  bitEditar = true;

  addressForm = this.fb.array([]);
  maxDate: Date;
  listaCronograma: ProcesoSeleccionCronograma[] = [];
  idProcesoSeleccion: number = 0;
  listaetapaActualProceso: Dominio[] = [];

  editorStyle = {
    height: '100px',
    width: '500px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }]
    ]
  };

  constructor(
    private fb: FormBuilder,
    private procesoSeleccionService: ProcesoSeleccionService,
    private activatedRoute: ActivatedRoute,
    public dialog: MatDialog,
    private commonService: CommonService
  ) {
    this.maxDate = new Date();
  }

  ngOnInit(): void {
    if (this.editMode.valor) this.addressForm.markAllAsTouched();
    this.addressForm = this.fb.array([]);

    this.commonService.listaEtapaActualProceso().subscribe(result => {
      this.listaetapaActualProceso = result;
    });
    this.activatedRoute.params.subscribe(parametro => {
      this.idProcesoSeleccion = parametro['id'];

      this.procesoSeleccionService
        .listaProcesoSeleccionCronogramaMonitoreo(this.idProcesoSeleccion)
        .subscribe(monitoreo => {
          // no tiene monitoreo ó los monitoreos ya estan cerrados
          let listaTemp = monitoreo.filter(
            r =>
              r.estadoActividadCodigo === this.estadosProcesoSeleccionMonitoreo.DevueltoPorComiteTecnico ||
              r.estadoActividadCodigo === this.estadosProcesoSeleccionMonitoreo.DevueltoPorComiteFiduciario ||
              r.estadoActividadCodigo === this.estadosProcesoSeleccionMonitoreo.AprobadoPorComiteTecnico ||
              r.estadoActividadCodigo === this.estadosProcesoSeleccionMonitoreo.EnTramite ||
              r.estadoActividadCodigo === this.estadosProcesoSeleccionMonitoreo.Creada
            //r.estadoActividadCodigo === this.estadosProcesoSeleccionMonitoreo.AprobadoPorComiteFiduciario
          );

          //console.log( listaTemp, monitoreo.filter( r => r.enviadoComiteTecnico != true ).length)

          this.procesoSeleccionService
            .listaActividadesByIdProcesoSeleccion(this.idProcesoSeleccion)
            .subscribe(lista => {
              let listaActividades = this.addressForm as FormArray;
              this.listaCronograma = lista;

              lista.forEach(cronograma => {
                let grupo = this.crearActividad();
                const etapaActualproceso = this.listaetapaActualProceso.find(
                  p => p.codigo === cronograma.etapaActualProcesoCodigo
                );
                grupo.get('procesoSeleccionCronogramaId').setValue(cronograma.procesoSeleccionCronogramaId);
                grupo.get('descripcion').setValue(cronograma.descripcion);
                grupo.get('fecha').setValue(cronograma.fechaMaxima);
                grupo.get('etapaActualProceso').setValue(etapaActualproceso), listaActividades.push(grupo);
              });
            });

          if (listaTemp.length === 0) {
          }
          // tiene monitoreos
          else {
            this.bitEditar = false;
            // let listaActividades = this.addressForm as FormArray;

            // this.listaCronograma = monitoreo[monitoreo.length-1].procesoSeleccionCronogramaMonitoreo.filter( r => r.eliminado !== true );
            // if(this.listaCronograma)
            // {

            //   this.listaCronograma.forEach(cronograma => {
            //     let grupo = this.crearActividad();
            //     const etapaActualproceso = this.listaetapaActualProceso.find(p => p.codigo === cronograma.etapaActualProcesoCodigo);
            //     grupo.get('procesoSeleccionCronogramaId').setValue(cronograma.procesoSeleccionCronogramaId);
            //     grupo.get('descripcion').setValue(cronograma.descripcion);
            //     grupo.get('fecha').setValue(cronograma.fechaMaxima);
            //     grupo.get('etapaActualProceso').setValue(etapaActualproceso),
            //     listaActividades.push(grupo);
            //     if(cronograma.estadoActividadCodigo!=EstadosSolicitudCronograma.Creada && cronograma.estadoActividadCodigo!=EstadosSolicitudCronograma.DevueltaPorComiteFiduciario &&
            //       cronograma.estadoActividadCodigo!=EstadosSolicitudCronograma.DevueltaPorComiteTecnico)
            //       {
            //         this.bitEditar=false;
            //       }
            //   })
            // }
          }
        });
      let listaProcesos: ProcesosElement[] = [];

      forkJoin([
        this.procesoSeleccionService.listaProcesoSeleccionCronogramaMonitoreo(this.idProcesoSeleccion),
        this.commonService.listaTipoProcesoSeleccion(),
        this.commonService.listaEstadoProcesoSeleccionMonitoreo(),
        this.commonService.listaEtapaProcesoSeleccion()
      ]).subscribe(respuesta => {
        respuesta[0].forEach(proceso => {
          console.log('proceso');
          console.log(proceso);
          let nombreTipo = respuesta[1].find(p => p.codigo == proceso.procesoSeleccion.tipoProcesoCodigo);
          let nombreEstado = respuesta[2].find(p => p.codigo == proceso.estadoActividadCodigo);
          let nombreEtapa = respuesta[3].find(p => p.codigo == proceso.estadoActividadCodigo);

          /*if (nombreTipo)   proceso.procesoSeleccion.tipoProcesoNombre = nombreTipo.nombre;
            if (nombreEstado) proceso.procesoSeleccion.estadoProcesoSeleccionNombre = nombreEstado.nombre;
            if (nombreEtapa)  proceso.procesoSeleccion.etapaProcesoSeleccionNombre = nombreTipo.nombre;

            */

          listaProcesos.push({
            estadoDelSolicitud: nombreEstado.nombre,
            fechaSolicitud: proceso.fechaCreacion.split('T')[0].split('-').reverse().join('/'),
            id: {
              estadoActividadCodigo: proceso.estadoActividadCodigo,
              numeroProceso: proceso.numeroProceso,
              procesoSeleccionCronogramaMonitoreo: proceso.procesoSeleccionCronogramaMonitoreo,
              procesoSeleccionId: proceso.procesoSeleccionId,
              procesoSeleccionMonitoreoId: proceso.procesoSeleccionMonitoreoId,
              fechaCreacion: proceso.fechaCreacion,
              usuarioCreacion: proceso.usuarioCreacion,
              eliminado: proceso.eliminado,
              enviadoComiteTecnico: proceso.enviadoComiteTecnico
            },
            numero: proceso.procesoSeleccion.numeroProceso,
            numeroSolicitud: proceso.numeroProceso,
            tipo: nombreTipo.nombre
          });
        });

        this.dataSource = new MatTableDataSource(listaProcesos);

        console.log(listaProcesos);

        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        this.paginator._intl.nextPageLabel = 'Siguiente';
        this.paginator._intl.previousPageLabel = 'Anterior';
      });
    });
  }

  agregaFuente() {
    this.addressForm.push(this.crearActividad());
    if (this.editMode.valor) this.addressForm.markAllAsTouched();
  }

  crearActividad(): FormGroup {
    return this.fb.group({
      procesoSeleccionCronogramaId: [],
      descripcion: [null, Validators.required],
      fecha: [null, Validators.required],
      etapaActualProceso: [null, Validators.required],
      procesoSeleccionCronogramaMonitoreoId: []
    });
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if (texto) {
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
  }

  borrarArray(borrarForm: any, i: number) {
    let listaActividades = this.addressForm as FormArray;
    console.log(listaActividades.controls[i].value);
    borrarForm.removeAt(i);
  }

  onSubmit() {
    this.bitEditar = true;

    let listaActividades = this.addressForm as FormArray;
    let listaCronograma: ProcesoSeleccionMonitoreo = {
      estadoActividadCodigo: null,
      numeroProceso: null,
      procesoSeleccionCronogramaMonitoreo: [],
      procesoSeleccionId: this.idProcesoSeleccion,
      procesoSeleccionMonitoreoId: this.procesoSeleccionMonitoreoId
    };

    let i = 0;
    listaActividades.controls.forEach(control => {
      let procesoSeleccionCronograma: ProcesoSeleccionCronogramaMonitoreo = {
        procesoSeleccionCronogramaMonitoreoId: control.get('procesoSeleccionCronogramaMonitoreoId').value,
        procesoSeleccionCronogramaId: control.get('procesoSeleccionCronogramaId').value,
        descripcion: control.get('descripcion').value,
        fechaMaxima: control.get('fecha').value,
        etapaActualProcesoCodigo: control.get('etapaActualProceso').value
          ? control.get('etapaActualProceso').value.codigo
          : null,
        //procesoSeleccionId: this.idProcesoSeleccion,
        numeroActividad: i
      };
      listaCronograma.procesoSeleccionCronogramaMonitoreo.push(procesoSeleccionCronograma);
      listaCronograma.procesoSeleccionId = this.idProcesoSeleccion;
      i++;
    });
    this.procesoSeleccionService
      .createEditarProcesoSeleccionCronogramaMonitoreo(listaCronograma)
      .subscribe(respuesta => {
        if (respuesta.code == '200') this.openDialog('', respuesta.message, true);
      });
  }

  openDialog(modalTitle: string, modalText: string, refrescar: boolean = false) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if (refrescar) {
      dialogRef.afterClosed().subscribe(result => {
        location.reload();
      });
    }
  }

  openDialogDevolucion() {
    this.dialog.open(DialogDevolucionComponent, {
      width: '70em'
    });
  }
  
  onDetalle(id: ProcesoSeleccionMonitoreo, tipo: number) {
    //this.editMode.valor = !this.editMode.valor;
    console.log('editar');
    console.log(id);
    let listaActividades = this.addressForm as FormArray;
    listaActividades.clear();
    id.procesoSeleccionCronogramaMonitoreo
      .filter(r => r.eliminado != true)
      .forEach(cronograma => {
        let grupo = this.crearActividad();
        const etapaActualproceso = this.listaetapaActualProceso.find(
          p => p.codigo === cronograma.etapaActualProcesoCodigo
        );
        grupo.get('procesoSeleccionCronogramaId').setValue(cronograma.procesoSeleccionCronogramaId);
        grupo.get('procesoSeleccionCronogramaMonitoreoId').setValue(cronograma.procesoSeleccionCronogramaMonitoreoId);
        grupo.get('descripcion').setValue(cronograma.descripcion);
        grupo.get('fecha').setValue(cronograma.fechaMaxima);
        grupo.get('etapaActualProceso').setValue(etapaActualproceso), listaActividades.push(grupo);
      });

    this.procesoSeleccionMonitoreoId = id.procesoSeleccionMonitoreoId;

    if (
      id.estadoActividadCodigo != EstadosSolicitudCronograma.Creada &&
      id.estadoActividadCodigo != EstadosSolicitudCronograma.DevueltaPorComiteFiduciario &&
      id.estadoActividadCodigo != EstadosSolicitudCronograma.DevueltaPorComiteTecnico
    ) {
      this.bitEditar = false;
    } else {
      this.bitEditar = true;
    }

    window.scroll(0, 0);
    //location.reload();
  }

  onEnviarSolicitud(id: any) {
    id.enviadoComiteTecnico = true;
    this.procesoSeleccionService.createEditarProcesoSeleccionCronogramaMonitoreo(id).subscribe(respuesta => {
      this.openDialog('', respuesta.message);
      if (respuesta.code == '200') this.ngOnInit();
    });
  }

  onEliminar(id: any) {
    this.openDialogSiNo('', '<b>¿Está seguro de eliminar este registro?</b>', id);
  }

  eliminarRegistro(id: ProcesoSeleccionCronogramaMonitoreo) {
    this.procesoSeleccionService
      .deleteProcesoSeleccionCronogramaMonitoreo(id.procesoSeleccionMonitoreoId)
      .subscribe(respuesta => {
        let r = respuesta as Respuesta;
        if (r.code == '200') {
          this.openDialog('', '<b>La información ha sido eliminada correctamente.</b>', true);
          //this.router.navigate(['/seleccion']);
        } else this.openDialog('', r.message);
      });
  }

  openDialogSiNo(modalTitle: string, modalText: string, id: any) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result === true) {
        this.eliminarRegistro(id);
      }
    });
  }
}
