import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { FormGestionarFuentesComponent } from '../form-gestionar-fuentes/form-gestionar-fuentes.component';

export interface PeriodicElement {
  id: number;
  llaveMen: string;
  tipoInterventor: string;
  departamento: string;
  municipio: string;
  institucion: string;
  sede: string;
  nombreAportante: string;
  valorAportante: number;
  estado: boolean;
}

const ELEMENT_DATA: PeriodicElement[] = [
  {
    id: 1,
    llaveMen: 'LL567444',
    tipoInterventor: 'Remodelación',
    departamento: 'Valle de Cauca',
    municipio: 'Jamundí',
    institucion: 'I.E Alfredo Bonilla Montaña',
    sede: 'Única sede',
    nombreAportante: 'FFIE',
    valorAportante: 200000000,
    estado: false
  },
  {
    id: 2,
    llaveMen: 'LL567444',
    tipoInterventor: 'Remodelación',
    departamento: 'Valle de Cauca',
    municipio: 'Jamundí',
    institucion: 'I.E Alfredo Bonilla Montaña',
    sede: 'Única sede',
    nombreAportante: 'Gobernación del Valle del Cauca',
    valorAportante: 200000000,
    estado: false
  },
];

@Component({
  selector: 'app-tabla-info-devuelta-por-coordinacion',
  templateUrl: './tabla-info-devuelta-por-coordinacion.component.html',
  styleUrls: ['./tabla-info-devuelta-por-coordinacion.component.scss']
})
export class TablaInfoDevueltaPorCoordinacionComponent implements OnInit {

  displayedColumns: string[] = [
    'llaveMen',
    'tipoInterventor',
    'departamento',
    'institucion',
    'nombreAportante',
    'valorAportante',
    'estado',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
    this.inicializarTabla();
  }
  inicializarTabla() {
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
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  gestionarFuentes(id: number) {
    console.log(id);
    // this.openDialog('', `El saldo actual de la fuente <b>Recursos propios</b> es menor
    // al valor solicitado de la fuente, verifique por favor.`);
    this.dialog.open(FormGestionarFuentesComponent, {
      width: '70em'
    });
  }

}
