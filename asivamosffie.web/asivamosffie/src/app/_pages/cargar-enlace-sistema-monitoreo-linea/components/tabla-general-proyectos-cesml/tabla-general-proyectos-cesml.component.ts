import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { DialogCargarSitioWebCesmlComponent } from '../dialog-cargar-sitio-web-cesml/dialog-cargar-sitio-web-cesml.component';

@Component({
  selector: 'app-tabla-general-proyectos-cesml',
  templateUrl: './tabla-general-proyectos-cesml.component.html',
  styleUrls: ['./tabla-general-proyectos-cesml.component.scss']
})
export class TablaGeneralProyectosCesmlComponent implements OnInit {
  @Input() dataTableServ: any;
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
  constructor(private router: Router, public dialog: MatDialog) { }

  ngOnInit(): void {
    let incompleto = 0;
    let completo = 0;
    for (let proy of this.dataTableServ) {
      if (proy.urlMonitoreo == "") {
        incompleto++;
      }
      if (proy.urlMonitoreo != "") {
        completo++;
      }
    }
    if (incompleto > 0 && completo < this.dataTableServ.length) {
      this.estadoSemaforo.emit('sin-diligenciar');
    }
    else if (completo > 0 && incompleto == 0) {
      this.estadoSemaforo.emit('completo');
    }
    console.log(incompleto);
    console.log(completo);
    this.dataSource = new MatTableDataSource(this.dataTableServ);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  configuracionSitioWeb(title,id, llaveMen, departamento, municipio, instEdu, sede, web) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '945px';
    dialogConfig.data = {title:title,id: id, llaveMen: llaveMen, departamento: departamento, municipio: municipio, instEdu: instEdu, sede: sede, web: web };
    const dialogRef = this.dialog.open(DialogCargarSitioWebCesmlComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(value => {
      if (value == 'aceptado') {
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/cargarEnlaceMonitoreoEnLinea'])
        );
      }
    });
  }
}
