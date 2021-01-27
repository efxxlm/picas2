import { Component, AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrar-informe-final-proyecto.service';

export interface RegistrarInterface {
  
  fechaTerminacionProyecto: Date,
  llaveMen: string,
  tipoIntervencion: string,
  institucionEducativa: string,
  sedeEducativa: string,
  contratoId: number,
  proyectoId: number,
  contratacionId: number,
  contratacionProyectoId: number,
  registroCompleto: boolean;
  estadoInforme: string
  
}

@Component({
  selector: 'app-tabla-informe-final-proyecto',
  templateUrl: './tabla-informe-final-proyecto.component.html',
  styleUrls: ['./tabla-informe-final-proyecto.component.scss']
})

export class TablaInformeFinalProyectoComponent implements OnInit, AfterViewInit {
  ELEMENT_DATA : RegistrarInterface[] = [];
  displayedColumns: string[] = [
    'fechaTerminacionProyecto',
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sedeEducativa',
    'estadoInforme',
    'registroCompleto',
    'contratacionProyectoId'
  ];
  dataSource = new MatTableDataSource<RegistrarInterface>(this.ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private registrarInformeFinalProyectoService: RegistrarInformeFinalProyectoService
  ) { }

  ngOnInit(): void {
    this.getAllReports();
  }

  getAllReports(){
    this.registrarInformeFinalProyectoService.getListReportGrilla()
    .subscribe(report => {
      this.dataSource.data = report as RegistrarInterface[];
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

}