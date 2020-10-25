import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CompromisosActasComiteService } from '../../../../core/_services/compromisosActasComite/compromisos-actas-comite.service';

@Component({
  selector: 'app-tabla-detalle-compromiso',
  templateUrl: './tabla-detalle-compromiso.component.html',
  styleUrls: ['./tabla-detalle-compromiso.component.scss']
})
export class TablaDetalleCompromisoComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @Input() compromisoId: number;
  @Input() tipoCompromiso: number;
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort: MatSort;
  displayedColumns: string[] = [ 'fechaCreacion', 'descripcionSeguimiento', 'estadoCompromiso' ];

  constructor ( private compromisoSvc: CompromisosActasComiteService ) {
  }

  ngOnInit(): void {
    this.getDataTable( this.compromisoId, this.tipoCompromiso );
  }

  getDataTable ( compromisoId: number, tipoCompromiso: number ) {
    console.log( compromisoId, tipoCompromiso );
    this.compromisoSvc.getCompromiso( compromisoId, tipoCompromiso )
      .subscribe( ( resp: any ) => {
        console.log( resp );
        this.dataSource = new MatTableDataSource( resp );
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      } );

  };

  textoLimpioMessage(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

}
