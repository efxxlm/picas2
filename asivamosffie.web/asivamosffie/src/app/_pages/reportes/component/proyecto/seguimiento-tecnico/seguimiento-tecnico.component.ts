import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ActivatedRoute, Params } from '@angular/router';
import { FichaProyectoService } from 'src/app/core/_services/fichaProyecto/ficha-proyecto.service';

@Component({
  selector: 'app-seguimiento-tecnico',
  templateUrl: './seguimiento-tecnico.component.html',
  styleUrls: ['./seguimiento-tecnico.component.scss']
})
export class SeguimientoTecnicoComponent implements OnInit {

  proyectoId: number;
  dataSeguimiento: any = null;
  dataSeguimientoObra: any = null;
  openAcordeon = false;
  displayedColumnsDiario: string[] = [ 'fechaSeguimiento', 'disponibilidadPersonal', 'disponibilidadMaterial', 'disponibilidadEquipo', 'productividad' ];
  displayedColumnsSemanal: string[] = [ 'numeroSemana', 'periodo', 'estadoObra', 'programacionObra', 'avanceFisico' ];

  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private fichaProyectoService: FichaProyectoService,
    private route: ActivatedRoute,
  ) {
    this.route.params.subscribe((params: Params) => {
      this.proyectoId = params.id;
    });
  }

  ngOnInit(): void {
    this.fichaProyectoService.getInfoSeguimientoTecnicoByProyectoId(this.proyectoId).subscribe(response => {
      this.dataSeguimiento = response;
      this.dataSeguimientoObra =  this.dataSeguimiento?.seguimientoTecnico?.find(r => r.tipoSolicitudCodigo == '1');
    });
  }

  downloadPDF() {
    this.openAcordeon = true;
    setTimeout(() => {
      document.title='Seguimiento técnico '+this.dataSeguimiento?.infoProyecto?.llaveMen;
      window.print();
    }, 250);
    window.onafterprint = function(){
      window.location.reload();
    }
  }


}
