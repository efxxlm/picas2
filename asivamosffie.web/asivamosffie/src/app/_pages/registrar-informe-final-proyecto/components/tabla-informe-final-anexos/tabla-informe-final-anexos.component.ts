import { Component, AfterViewInit, ViewChild, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DialogTipoDocumentoComponent } from '../dialog-tipo-documento/dialog-tipo-documento.component';
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component';

import { Anexo } from 'src/app/_interfaces/proyecto-final-anexos.model';
import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrar-informe-final-proyecto.service';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-tabla-informe-final-anexos',
  templateUrl: './tabla-informe-final-anexos.component.html',
  styleUrls: ['./tabla-informe-final-anexos.component.scss']
})
export class TablaInformeFinalAnexosComponent implements OnInit, AfterViewInit {
  ELEMENT_DATA : Anexo[] = [];
  @Input() id: string;
  @Input() llaveMen: string;
  @Input() estadoInforme: string;
  @Input() report: Report;
  estadoInformeString: string;  
  estaEditando: boolean;

  anexos: any;
  displayedColumns: string[] = [
    'informeFinalListaChequeoId',
    'nombre',
    'calificacionCodigo',
    'informeFinalInterventoriaId'
  ];
  addressForm: FormGroup;
  dataSource = new MatTableDataSource<Anexo>(this.ELEMENT_DATA);

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
    private registrarInformeFinalProyectoService: RegistrarInformeFinalProyectoService
  ) { }

  ngOnInit(): void {
    if (this.estadoInforme === '2' || this.estadoInforme === '3') { //con informe registrado/ enviado a validación
      this.estadoInformeString = 'completo';
    } else if (this.estadoInforme === '1' || this.estadoInforme === '4') { //en proceso de registro, con observaciones
      this.estadoInformeString = 'en-proceso';
    } else {
      this.estadoInformeString = 'sin-diligenciar';
    }

    this.stateEstaEditando();
    this.getInformeFinalListaChequeo(this.id);
  }

  stateEstaEditando() {
    this.report.proyecto.informeFinal.length > 0 ? (this.estaEditando = true) : (this.estaEditando = false);
  }

  getInformeFinalListaChequeo (id:string) {
    this.registrarInformeFinalProyectoService.getInformeFinalListaChequeo(id)
    .subscribe(anexos => {
      this.dataSource.data = anexos as Anexo[];
      this.anexos = anexos;
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
    let dialogRef = this.dialog.open(DialogTipoDocumentoComponent, {
      width: '70em',
      data:{
        informe: informe,
        llaveMen: this.llaveMen
      },
      id:'dialogTipoDocumento'
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      this.ngOnInit();
      return;
    });
  }

  openDialogObservaciones(informe:any) {
    let dialogRef = this.dialog.open(DialogObservacionesComponent, {
      width: '70em',
      data: {
        informe: informe,
        llaveMen: this.llaveMen
      },
      id:'dialogObservaciones'
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      this.ngOnInit();
      return;
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText },
    });
  }

  onSubmit() {
    this.estaEditando = true;
    if(this.addressForm !== undefined){
      this.verificarInformeFinalEstadoCompleto(this.addressForm.value.informeFinalId);
    }else{
      this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
    }
  }

  select(informeFinalAnexo) {
    this.addressForm = this.fb.group({
      calificacionCodigo:  [informeFinalAnexo.calificacionCodigo, Validators.required],
      informeFinalId:  [informeFinalAnexo.informeFinalId, Validators.required],
      informeFinalInterventoriaId:  [informeFinalAnexo.informeFinalInterventoriaId, Validators.required],
      informeFinalListaChequeoId:  [informeFinalAnexo.informeFinalListaChequeoId, Validators.required],
      posicion:  [informeFinalAnexo.posicion, Validators.required],
    });
    this.createInformeFinalInterventoria(this.addressForm.value);
  }

  createInformeFinalInterventoria( informeFinalInterventoria: any ) {
    this.registrarInformeFinalProyectoService.createEditInformeFinalInterventoria(informeFinalInterventoria)
    .subscribe((respuesta: Respuesta) => {
        console.log(respuesta.message);
        this.ngOnInit();
        return;
      },
      err => {
        console.log( err );
      });
  }
  

  verificarInformeFinalEstadoCompleto( informeFinalId: number ) {
    this.registrarInformeFinalProyectoService.verificarInformeFinalEstadoCompleto(informeFinalId)
    .subscribe(respuesta => {
        if(respuesta != null){
          this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
        }
        return;
      },
      err => {
        console.log( err );
      });
  }

}