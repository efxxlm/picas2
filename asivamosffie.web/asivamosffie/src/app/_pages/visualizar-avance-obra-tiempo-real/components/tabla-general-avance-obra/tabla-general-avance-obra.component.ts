import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-general-avance-obra',
  templateUrl: './tabla-general-avance-obra.component.html',
  styleUrls: ['./tabla-general-avance-obra.component.scss']
})
export class TablaGeneralAvanceObraComponent implements OnInit {
  @Input () dataTableServ:any;
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'llaveMen',
    'tipoIntervencion',
    'region',
    'departamento',
    'municipio',
    'institucionEducativa',
    'sede',
    'gestion'
  ];
  dataTable: any[] = [
    {
      llaveMen: 'LJ776554',
      tipoIntervencion: 'Remodelación',
      region: 'Caribe',
      departamento: 'Atlántico',
      municipio: 'Malambo',
      institucionEducativa: 'I.E. María Villa Campo',
      sede: 'Única sede',
      sitioWeb: 'http://www.ffie.com',
      id: 1
    },
    {
      llaveMen: 'LU990088',
      tipoIntervencion: 'Remodelación',
      region: 'Caribe',
      departamento: 'Atlántico',
      municipio: 'Baranoa',
      institucionEducativa: 'María Inmaculada',
      sede: 'Sede 2',
      sitioWeb: 'http://www.ffie.com',
      id: 2
    },
    {
      llaveMen: 'LY665533',
      tipoIntervencion: 'Remodelación',
      region: 'Caribe',
      departamento: 'Atlántico',
      municipio: 'Soledad',
      institucionEducativa: 'I.E. Primera de Mayo',
      sede: 'Única sede',
      sitioWeb: 'http://www.ffie.com',
      id: 3
    }
  ];
  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTableServ);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  irSitioWeb(web){
    window.open(web,'_blank');
  }

}
