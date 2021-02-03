import { Component, AfterViewInit, ViewChild, OnInit, Input } from '@angular/core'
import { MatPaginator } from '@angular/material/paginator'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'
import { MatDialog } from '@angular/material/dialog'
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms'
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component'
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component';

const ELEMENT_DATA = [
  {
    No: '1',
    item: 'Acta de recibo a satisfacción de la fase 2 - Interventoría',
    calificacionInterventoria: 'Cumple',
    tipoAnexo: 'Cumple',
    Ubicacion: 'https://drive.google.com/drive/actadereciboasatisfaccion',
    verificacion: 'No cumple',
    id: '1'
  }
]

@Component({
  selector: 'app-tabla-informe-final-anexos',
  templateUrl: './tabla-informe-final-anexos.component.html',
  styleUrls: ['./tabla-informe-final-anexos.component.scss']
})
export class TablaInformeFinalAnexosComponent implements OnInit, AfterViewInit {
  ELEMENT_DATA: any[]
  llaveMen = "LJ77654";
  displayedColumns: string[] = [
    'item',
    'calificacionInterventoria',
    'tipoAnexo',
    'Ubicacion',
    'verificacion',
    'id'
  ]
  dataSource = new MatTableDataSource(ELEMENT_DATA)

  @ViewChild(MatPaginator) paginator: MatPaginator
  @ViewChild(MatSort) sort: MatSort
  constructor(
    public dialog: MatDialog
  ) {}

  estaEditando = false

  estadoArray = [
    { name: 'Cumple', value: 'Cumple' },
    { name: 'No cumple', value: 'No cumple' },
    { name: 'No aplica', value: 'No aplica' }
  ]

  ngOnInit(): void {}

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

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText },
    });
  }

  openDialogTipoDocumento(informe:any) {
    // let dialogRef = this.dialog.open(DialogTipoDocumentoComponent, {
    //   width: '70em',
    //   data:{
    //     informe: informe,
    //     llaveMen: this.llaveMen
    //   }
    // });

    // dialogRef.afterClosed().subscribe(result => {
    //   console.log(`Dialog result: ${result}`);
    // });
  }

  openDialogObservaciones(informe:any) {
    let dialogRef = this.dialog.open(DialogObservacionesComponent, {
      width: '70em',
      data: {
        informe: informe,
        llaveMen: this.llaveMen
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

  onSubmit() {
    console.log(ELEMENT_DATA);
    this.estaEditando = true;
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }
}
