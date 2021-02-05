import { Component, AfterViewInit, ViewChild, OnInit, Input } from '@angular/core'
import { MatPaginator } from '@angular/material/paginator'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'
import { MatDialog } from '@angular/material/dialog'
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms'
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component'
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component';
import { ValidarInformeFinalService } from 'src/app/core/_services/validarInformeFinal/validar-informe-final.service'
import { Anexo } from 'src/app/_interfaces/proyecto-final-anexos.model'
import { Respuesta } from 'src/app/core/_services/common/common.service'

@Component({
  selector: 'app-tabla-informe-final-anexos',
  templateUrl: './tabla-informe-final-anexos.component.html',
  styleUrls: ['./tabla-informe-final-anexos.component.scss']
})
export class TablaInformeFinalAnexosComponent implements OnInit, AfterViewInit {
  ELEMENT_DATA : Anexo[] = [];
  @Input() id: number;
  @Input() llaveMen: string;
  anexos: any;
  displayedColumns: string[] = [
    'No',
    'item',
    'calificacionCodigoString',
    'tipoAnexoString',
    'Ubicacion',
    'verificacion',
    'id'
  ];
  addressForm: FormGroup;
  dataSource = new MatTableDataSource<Anexo>(this.ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator
  @ViewChild(MatSort) sort: MatSort
  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private validarInformeFinalService: ValidarInformeFinalService
  ) {}

  estaEditando = false

  estadoArray = [
    { name: 'Cumple', value: 1 },
    { name: 'No cumple', value: 2 },
    { name: 'No aplica', value: 3 }
  ]

  ngOnInit(): void {
    this.getInformeFinalListaChequeoByInformeFinalId(this.id);
  }

  getInformeFinalListaChequeoByInformeFinalId (id:number) {
    this.validarInformeFinalService.getInformeFinalListaChequeoByInformeFinalId(id)
    .subscribe(anexos => {
      this.dataSource.data = anexos as Anexo[];
      this.anexos = anexos;
      console.log("Aquí:",this.anexos);
    });
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort
    this.dataSource.paginator = this.paginator
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página'
    this.paginator._intl.nextPageLabel = 'Siguiente'
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length
      }
      length = Math.max(length, 0)
      const startIndex = page * pageSize
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length
    }
    this.paginator._intl.previousPageLabel = 'Anterior'
  }

  openDialogObservaciones(informe:any) {
    let dialogRef = this.dialog.open(DialogObservacionesComponent, {
      width: '70em',
      data: {
        informe: informe,
        llaveMen: this.llaveMen
      },
      id:'dialogObservacionesSupervisor'
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
    // console.log(this.addressForm.value);
    this.estaEditando = true;
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }
  //Actualizar validación
  select(element) {
    console.log("antes de: ",element);
    this.addressForm = this.fb.group({
      calificacionCodigo:  [element.calificacionCodigo, Validators.required],
      informeFinalId:  [element.informeFinalId, Validators.required],
      informeFinalInterventoriaId:  [element.informeFinalInterventoriaId, Validators.required],
      informeFinalListaChequeoId:  [element.informeFinalListaChequeoId, Validators.required],
      validacionCodigo: [element.verificacion, Validators.required]
    });
    console.log("Autosave test: ",this.addressForm.value);
    this.updateStateValidateInformeFinalInterventoria(this.addressForm.value.informeFinalInterventoriaId, this.addressForm.value.validacionCodigo);
  }


  updateStateValidateInformeFinalInterventoria( id: number, code: string ) {
    this.validarInformeFinalService.updateStateValidateInformeFinalInterventoria(id, code)
    .subscribe((respuesta: Respuesta) => {
        console.log(respuesta.message);
        this.ngOnInit();
        return;
      },
      err => {
        console.log( err );
      });
  }
}
