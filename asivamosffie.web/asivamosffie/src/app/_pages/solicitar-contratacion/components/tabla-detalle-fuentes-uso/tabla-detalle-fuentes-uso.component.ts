import { Dominio } from './../../../../core/_services/common/common.service';
import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-tabla-detalle-fuentes-uso',
  templateUrl: './tabla-detalle-fuentes-uso.component.html',
  styleUrls: ['./tabla-detalle-fuentes-uso.component.scss']
})
export class TablaDetalleFuentesUsoComponent implements OnInit {

  @Input() contratacionProyecto: any;
  @Input() contratacionProyectoAportanteId: any;
  listaFuenteTipoFinanciacion: Dominio[] = [];
  listaFuenteFinanciacion = [];
  
  dataSource = new MatTableDataSource();
  dataTable: any[] = [];
  displayedColumns: string[] = [     
    'componente',
    'fase',
    'tipoUso',
    'fuente',
    'valorUso'
  ]

  constructor( private commonSvc: CommonService, ) { }

  async ngOnInit() {
    this.listaFuenteTipoFinanciacion = await this.commonSvc.listaFuenteTipoFinanciacion().toPromise();

    this.contratacionProyecto.contratacionProyectoAportante.forEach( contratacionProyectoAportante => {
      contratacionProyectoAportante.cofinanciacionAportante.fuenteFinanciacion.forEach( fuenteFinanciacion => this.listaFuenteFinanciacion.push( fuenteFinanciacion ) );
    } )

    setTimeout(() => {
      if ( this.contratacionProyecto.dataAportantes.length > 0 ) {
        this.dataTable = this.contratacionProyecto.dataAportantes.filter( da => da.contratacionProyectoAportanteId === this.contratacionProyectoAportanteId );
        this.dataSource = new MatTableDataSource ( this.dataTable );
      }
    }, 1000);
  };

  getFuenteFinanciacion( fuenteFinanciacionId ) {
    if ( this.listaFuenteTipoFinanciacion.length > 0 && this.listaFuenteFinanciacion.length > 0 ) {
      const fuente = this.listaFuenteFinanciacion.find( fuente => fuente.fuenteFinanciacionId === fuenteFinanciacionId );

      if ( fuente !== undefined ) {
        const fuenteFinanciacion = this.listaFuenteTipoFinanciacion.find( fuenteFinanciacion => fuenteFinanciacion.codigo === fuente.fuenteRecursosCodigo );

        if ( fuenteFinanciacion !== undefined ) {
          return fuenteFinanciacion.nombre;
        }
      }
    }
  }

}
