import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MonitoringURLService } from 'src/app/core/_services/monitoringURL/monitoring-url.service';

@Component({
  selector: 'app-acordion-tabla-lista-proyectos-vaotr',
  templateUrl: './acordion-tabla-lista-proyectos-vaotr.component.html',
  styleUrls: ['./acordion-tabla-lista-proyectos-vaotr.component.scss']
})
export class AcordionTablaListaProyectosVaotrComponent implements OnInit {
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
