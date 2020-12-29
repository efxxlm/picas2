import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DialogCargarSitioWebCesmlComponent } from '../dialog-cargar-sitio-web-cesml/dialog-cargar-sitio-web-cesml.component';

@Component({
  selector: 'app-tabla-general-proyectos-cesml',
  templateUrl: './tabla-general-proyectos-cesml.component.html',
  styleUrls: ['./tabla-general-proyectos-cesml.component.scss']
})
export class TablaGeneralProyectosCesmlComponent implements OnInit {
  @Input () dataTableServ:any;
  @Output() estadoSemaforo = new EventEmitter<string>();
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
      sitioWeb: '',
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
      sitioWeb: '',
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
    let incompleto = 0;
    let completo = 0;
    for(let proy of this.dataTableServ){
      if(proy.urlMonitoreo=="" || proy.urlMonitoreo==null || proy.urlMonitoreo==undefined){
        incompleto++;
      }
      else{
        completo++;
      }
    }
    if(completo == this.dataTableServ.length){
      this.estadoSemaforo.emit('completo');
    }
    else{
      this.estadoSemaforo.emit('sin-diligenciar');
    }
    this.dataSource = new MatTableDataSource(this.dataTableServ);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  configuracionSitioWeb(id, llaveMen, departamento, municipio, instEdu, sede, web){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '45%';
    dialogConfig.data = {id:id, llaveMen:llaveMen, departamento:departamento, municipio:municipio, instEdu:instEdu, sede:sede, web:web};
    const dialogRef = this.dialog.open(DialogCargarSitioWebCesmlComponent, dialogConfig);
  }
}
