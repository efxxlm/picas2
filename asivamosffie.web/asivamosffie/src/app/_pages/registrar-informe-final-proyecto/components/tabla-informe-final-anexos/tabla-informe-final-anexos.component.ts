import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DialogTipoDocumentoComponent } from '../dialog-tipo-documento/dialog-tipo-documento.component';

export interface VerificacionDiaria {
  id: string;
  numero: string;
  item: string;
  estadoRequisito: string;
}

const ELEMENT_DATA: VerificacionDiaria[] = [
  {
    id: '1',
    numero: '21/06/2020',
    item: 'LJ776554',
    estadoRequisito: '',
  }
];

@Component({
  selector: 'app-tabla-informe-final-anexos',
  templateUrl: './tabla-informe-final-anexos.component.html',
  styleUrls: ['./tabla-informe-final-anexos.component.scss']
})
export class TablaInformeFinalAnexosComponent implements AfterViewInit {

  displayedColumns: string[] = [
    'numero',
    'item',
    'estadoRequisito',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  addressForm = this.fb.group({
    informe: this.fb.array([
      [null, Validators.required]
    ])
  })

  estaEditando = false;

  estadoArray = [
    { name: 'Cumple', value: 'cumple' },
    { name: 'No cumple', value: 'noCumple' },
    { name: 'No aplica', value: 'noAplica' },
  ];

  get informeField() {
    return this.addressForm.get('informe') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog
  ) { }

  ngAfterViewInit() {
    // this.dataSource.sort = this.sort;
    // this.dataSource.paginator = this.paginator;
    // this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    // this.paginator._intl.nextPageLabel = 'Siguiente';
    // this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
    //   return (page + 1).toString() + ' de ' + length.toString();
    // };
    // this.paginator._intl.previousPageLabel = 'Anterior';
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openDialogTipoDocumento() {
    let dialogRef = this.dialog.open(DialogTipoDocumentoComponent, {
      width: '70em'
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText },
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.estaEditando = true;
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }

}