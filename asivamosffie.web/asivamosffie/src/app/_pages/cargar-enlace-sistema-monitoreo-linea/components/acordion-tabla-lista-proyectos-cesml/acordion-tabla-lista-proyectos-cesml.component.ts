import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MonitoringURLService } from 'src/app/core/_services/monitoringURL/monitoring-url.service';
import { DialogCargarSitioWebCesmlComponent } from '../dialog-cargar-sitio-web-cesml/dialog-cargar-sitio-web-cesml.component';

@Component({
  selector: 'app-acordion-tabla-lista-proyectos-cesml',
  templateUrl: './acordion-tabla-lista-proyectos-cesml.component.html',
  styleUrls: ['./acordion-tabla-lista-proyectos-cesml.component.scss']
})
export class AcordionTablaListaProyectosCesmlComponent implements OnInit {
  public dataContrato;
  constructor(public dialog: MatDialog, private services: MonitoringURLService) { }

  ngOnInit(): void {
    this.loadService();

  }

  loadService(){
    this.services.GetListContratoProyectos().subscribe(data=>{
      this.loadData(data);
    });
  }

  loadData(data){
    this.dataContrato = data;
    console.log(this.dataContrato);
  }

}
