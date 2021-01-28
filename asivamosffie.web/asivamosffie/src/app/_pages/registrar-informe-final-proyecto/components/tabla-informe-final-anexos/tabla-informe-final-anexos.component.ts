import { Component, AfterViewInit, ViewChild, OnInit, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DialogTipoDocumentoComponent } from '../dialog-tipo-documento/dialog-tipo-documento.component';

import { Anexo } from 'src/app/_interfaces/proyecto-final-anexos.model';
import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrar-informe-final-proyecto.service';


@Component({
  selector: 'app-tabla-informe-final-anexos',
  templateUrl: './tabla-informe-final-anexos.component.html',
  styleUrls: ['./tabla-informe-final-anexos.component.scss']
})
export class TablaInformeFinalAnexosComponent implements OnInit, AfterViewInit {
  ELEMENT_DATA : Anexo[] = [];
  @Input() id: string;
  anexos: any;
  displayedColumns: string[] = [
    'informeFinalListaChequeoId',
    'nombre',
    'calificacionCodigo',
    'informeFinalInterventoriaId'
  ];
  dataSource = new MatTableDataSource<Anexo>(this.ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  // addressForm = this.fb.group({
  //   informe: this.fb.array([
  //     [null, Validators.required]
  //   ])
  // })

  estaEditando = false;

  estadoArray = [
    { name: 'Cumple', value: 1 },
    { name: 'No cumple', value: 2 },
    { name: 'No aplica', value: 3 ,}
  ];

  // get informeField() {
  //   return this.addressForm.get('informe') as FormArray;
  // }

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private registrarInformeFinalProyectoService: RegistrarInformeFinalProyectoService
  ) { }

  ngOnInit(): void {
    this.getInformeFinalListaChequeo(this.id);
  }

  getInformeFinalListaChequeo (id:string) {
    this.registrarInformeFinalProyectoService.getInformeFinalListaChequeo(id)
    .subscribe(anexos => {
      this.dataSource.data = anexos as Anexo[];
      this.anexos = anexos;
      console.log(this.anexos);
    });
  }



  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      return (page + 1).toString() + ' de ' + length.toString();
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
    console.log("estoy probando: ",informe);
    let dialogRef = this.dialog.open(DialogTipoDocumentoComponent, {
      width: '70em',
      data:{
        informe: informe,
      }
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
    // console.log(this.addressForm.value);
    this.estaEditando = true;
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }

}