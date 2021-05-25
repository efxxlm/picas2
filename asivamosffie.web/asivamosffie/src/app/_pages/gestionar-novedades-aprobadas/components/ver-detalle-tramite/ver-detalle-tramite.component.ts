import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual, NovedadContractualAportante } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-ver-detalle-tramite',
  templateUrl: './ver-detalle-tramite.component.html',
  styleUrls: ['./ver-detalle-tramite.component.scss']
})
export class VerDetalleTramiteComponent implements OnInit {

  detalleId: string;
  novedad: NovedadContractual;
  tieneAdicion: boolean = false;
  esNoFirma: boolean = false;
  dataSource = new MatTableDataSource();
  dataTable: any[] = [];

  detallarSolicitud = [];
  displayedColumns: string[] = [
    'aportante',
    'valorAportante',
    'componente',    
    'fuenteAportante',
    'uso',    
    'valorUso'
  ];
  tipoNovedadNombre: string = '';

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private contractualNoveltyService: ContractualNoveltyService,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.detalleId = params.id;

      this.contractualNoveltyService.getNovedadContractualById(this.detalleId)
        .subscribe(respuesta => {
          this.novedad = respuesta;

          respuesta.novedadContractualDescripcion.forEach(d => {
            if (d.tipoNovedadCodigo === '3')
              this.tieneAdicion = true;

            if(d.tipoNovedadCodigo === '3'|| d.tipoNovedadCodigo === '4' || d.tipoNovedadCodigo === '5')
              this.esNoFirma = true;

            this.tipoNovedadNombre = this.tipoNovedadNombre + d.nombreTipoNovedad + ', ' 
          });


          if (this.tieneAdicion === true) {
            if(this.novedad.novedadContractualAportante.length>0){
              this.dataTable = this.novedad.novedadContractualAportante;
              this.dataSource = new MatTableDataSource ( this.dataTable );
            }
          }
        });
    });
  }

}
