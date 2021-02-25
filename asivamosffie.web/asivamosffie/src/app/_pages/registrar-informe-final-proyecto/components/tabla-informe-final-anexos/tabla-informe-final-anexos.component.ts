import { Component, AfterViewInit, ViewChild, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DialogTipoDocumentoComponent } from '../dialog-tipo-documento/dialog-tipo-documento.component';
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component';

import { ListaChequeo } from 'src/app/_interfaces/proyecto-final-anexos.model';
import { InformeFinal, InformeFinalAnexo, InformeFinalInterventoria, InformeFinalInterventoriaObservaciones } from 'src/app/_interfaces/informe-final';
import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrarInformeFinal/registrar-informe-final-proyecto.service'
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { Report } from 'src/app/_interfaces/proyecto-final.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-informe-final-anexos',
  templateUrl: './tabla-informe-final-anexos.component.html',
  styleUrls: ['./tabla-informe-final-anexos.component.scss']
})
export class TablaInformeFinalAnexosComponent implements OnInit, AfterViewInit {
  ELEMENT_DATA : ListaChequeo[] = [];
  @Input() id: string;
  @Input() llaveMen: string;
  @Input() report: Report;
  estaEditando: boolean;

  estadoInforme = '0';
  registroCompleto = false;
  semaforo= false;
  editadoSupervision = false;
  noGuardado=false;

  listChequeo: any;
  displayedColumns: string[] = [
    'informeFinalListaChequeoId',
    'nombre',
    'calificacionCodigo',
    'informeFinalInterventoriaId'
  ];
  informeFinalObservacion : InformeFinalInterventoriaObservaciones[] = [];
  informeFinalAnexo : InformeFinalAnexo;
  //addressForm: FormGroup;
  addressForm = this.fb.group({});
  dataSource = new MatTableDataSource<ListaChequeo>(this.ELEMENT_DATA);

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
    private registrarInformeFinalProyectoService: RegistrarInformeFinalProyectoService,
    private router: Router  ) { }
  


  ngOnInit(): void {
    this.stateEstaEditando();
    this.getInformeFinalListaChequeo(this.id);
  }

  stateEstaEditando() {
    this.report.proyecto.informeFinal.length > 0 ? (this.estaEditando = true) : (this.estaEditando = false);
  }
  
  getInformeFinalListaChequeo (id:string) {
    this.registrarInformeFinalProyectoService.getInformeFinalListaChequeo(id)
    .subscribe(listChequeo => {
      if(listChequeo != null && listChequeo.length> 0){
        this.estadoInforme = listChequeo[0].estadoInforme;
        this.registroCompleto = listChequeo[0].registroCompleto;
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

  openDialogTipoDocumento(informe:any) {
    this.informeFinalAnexo = null;
    this.dataSource.data.forEach(control => {
      if ( informe !== null && informe.informeFinalInterventoriaId === control.informeFinalInterventoriaId ) {
        if(control.informeFinalAnexo != null){
          this.informeFinalAnexo = control.informeFinalAnexo;
        }
        return;
      }
    });
    let dialogRef = this.dialog.open(DialogTipoDocumentoComponent, {
      width: '70em',
      data:{
        informe: informe,
        llaveMen: this.llaveMen,
        informeFinalAnexo: this.informeFinalAnexo,
      },
      id:'dialogTipoDocumento'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.dataSource.data.forEach(control => {
        if ( result !== null && result.id === control.informeFinalInterventoriaId ) {
          this.editadoSupervision = true;
          const informeFinalAnexo: InformeFinalAnexo = {
            informeFinalAnexoId: result.anexo.informeFinalAnexoId,
            tipoAnexo: result.anexo.tipoAnexo,
            numRadicadoSac: result.anexo.numRadicadoSac,
            fechaRadicado: result.anexo.fechaRadicado,
            urlSoporte: result.anexo.urlSoporte,
          };
          control.tieneAnexo = true;
          control.informeFinalAnexo = informeFinalAnexo;
          return;
        }
      });
      return;
    });
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
            esCalificacion: result.observaciones.esCalificacion
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
      this.router.navigate(['/registrarInformeFinalProyecto']);
    });
  }


  onSubmit(test: boolean) {
    this.estaEditando = true;
    this.noGuardado=false;
    //recorre el datasource y crea modelo
    const listaInformeFinalInterventoria = [] as InformeFinal;
    listaInformeFinalInterventoria.informeFinalInterventoria = [];

    this.dataSource.data.forEach(control => {
        const informeFinalInterventoria: InformeFinalInterventoria = {
          informeFinalInterventoriaId: control.informeFinalInterventoriaId,
          informeFinalId: control.informeFinalId,
          calificacionCodigo: control.calificacionCodigo,
          informeFinalListaChequeoId: control.informeFinalListaChequeoId,
          informeFinalAnexo: control.informeFinalAnexo,
          informeFinalInterventoriaObservaciones: control.informeFinalInterventoriaObservaciones,
        };
        listaInformeFinalInterventoria.informeFinalInterventoria.push(informeFinalInterventoria);
    });
    const informeFinal: InformeFinal = {
      informeFinalId: this.dataSource.data[0].informeFinalId,
      proyectoId: Number(this.id),
      informeFinalInterventoria: listaInformeFinalInterventoria.informeFinalInterventoria,
    };
    this.createEditInformeFinalInterventoriabyInformeFinal(informeFinal, test);
  }

  createEditInformeFinalInterventoriabyInformeFinal( informeFinal: any, test: boolean) {
    this.registrarInformeFinalProyectoService.createEditInformeFinalInterventoriabyInformeFinal(informeFinal)
    .subscribe((respuesta: Respuesta) => {
      if(!test){
        this.openDialogSuccess('', respuesta.message);
      }else{
        this.openDialog('', respuesta.message);
      }
        //this.ngOnInit();
        return; 
      },
      err => {
        this.openDialog('', err.message);
        this.ngOnInit();
        return;
      });
  }

  changeState(){
    this.noGuardado = true;
  }

  doSomething() {
    console.log('do something');
  }
}