import { Component, AfterViewInit, ViewChild, OnInit, Input } from '@angular/core';
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

@Component({
  selector: 'app-tabla-detalle-informe-anexos',
  templateUrl: './tabla-detalle-informe-anexos.component.html',
  styleUrls: ['./tabla-detalle-informe-anexos.component.scss']
})
export class TablaDetalleInformeAnexosComponent implements OnInit, AfterViewInit {

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

  constructor(
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
      console.log("Aquí:",this.anexos);
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
    let dialogRef = this.dialog.open(DialogTipoDocumentoComponent, {
      width: '70em',
      data:{
        informe: informe
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

}
