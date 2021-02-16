import { Component, AfterViewInit, ViewChild, OnInit, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
// import { DialogTipoDocumentoComponent } from '../dialog-tipo-documento/dialog-tipo-documento.component';
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component';

import { ListaChequeo } from 'src/app/_interfaces/proyecto-final-anexos.model';
import { InformeFinal, InformeFinalAnexo, InformeFinalInterventoria, InformeFinalInterventoriaObservaciones } from 'src/app/_interfaces/informe-final';
import { ValidarInformeFinalService } from 'src/app/core/_services/validarInformeFinal/validar-informe-final.service'
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-detalle-informe-anexos',
  templateUrl: './tabla-detalle-informe-anexos.component.html',
  styleUrls: ['./tabla-detalle-informe-anexos.component.scss']
})
export class TablaDetalleInformeAnexosComponent implements OnInit, AfterViewInit {
  ELEMENT_DATA : ListaChequeo[] = [];
  @Input() id: number;
  @Input() llaveMen: string;
  listChequeo: any;
  displayedColumns: string[] = [
    'No',
    'item',
    'calificacionCodigoString',
    'tipoAnexoString',
    'Ubicacion',
    'validacionCodigoString',
    'aprobacionCodigo',
    'id'
  ];
  estadoAprobacion = '0';
  //registroCompletoValidacion = false;
  addressForm: FormGroup;
  informeFinalObservacion : InformeFinalInterventoriaObservaciones[] = [];
  dataSource = new MatTableDataSource<ListaChequeo>(this.ELEMENT_DATA);
  semaforo = false;
  estaEditando = false;
  noGuardado=false;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  

  estadoArray = [
    { name: 'Cumple', value: 1 },
    { name: 'No cumple', value: 2 },
    { name: 'No aplica', value: 3 ,}
  ];

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private validarInformeFinalProyectoService: ValidarInformeFinalService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.getInformeFinalListaChequeoByInformeFinalId(this.id);
  }

  getInformeFinalListaChequeoByInformeFinalId (id:number) {
    this.validarInformeFinalProyectoService.getInformeFinalListaChequeoByInformeFinalId(id)
    .subscribe(listChequeo => {
      if(listChequeo != null){
        this.estadoAprobacion = listChequeo[0].estadoAprobacion;
        //this.registroCompletoValidacion = listChequeo[0].registroCompletoValidacion;
        this.semaforo = listChequeo[0].semaforo;
      }
      this.dataSource.data = listChequeo as ListaChequeo[];
      this.listChequeo = listChequeo;
    });
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openDialogObservaciones(informe:any) {
    this.informeFinalObservacion = null;
    this.dataSource.data.forEach(control => {
      if ( informe !== null && informe.informeFinalInterventoriaId === control.informeFinalInterventoriaId ) {
        if(control.informeFinalInterventoriaObservaciones != null && control.informeFinalInterventoriaObservaciones.length > 0){
          this.informeFinalObservacion = control.informeFinalInterventoriaObservaciones;
        }
        return;
      }
    });
    let dialogRef = this.dialog.open(DialogObservacionesComponent, {
      width: '70em',
      data: {
        informe: informe,
        llaveMen: this.llaveMen,
        informeFinalObservacion: this.informeFinalObservacion,
      },
      id:'dialogObservaciones'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.dataSource.data.forEach(control => {
        if ( result !== null && result.id === control.informeFinalInterventoriaId ) {
          control.informeFinalInterventoriaObservaciones = [];
          const informeFinalInterventoriaObservaciones: InformeFinalInterventoriaObservaciones = {
            informeFinalInterventoriaObservacionesId:result.observaciones.informeFinalInterventoriaObservacionesId,
            informeFinalInterventoriaId:result.observaciones.informeFinalInterventoriaId,
            observaciones: result.observaciones.observaciones,
            esSupervision: result.observaciones.esSupervision,
            esCalificacion: result.observaciones.esCalificacion,
            esApoyo: result.observaciones.esApoyo,
          };
          control.tieneObservacionNoCumple = true;
          control.informeFinalInterventoriaObservaciones.push(informeFinalInterventoriaObservaciones);
          return;
        }
      });
      return;
    });
  }


  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText },
    });
  }

  openDialogSuccess(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText },
    });
  
    dialogRef.afterClosed().subscribe(result => {
      this.router.navigate(['/validarInformeFinalProyecto']);
    });
  }


}