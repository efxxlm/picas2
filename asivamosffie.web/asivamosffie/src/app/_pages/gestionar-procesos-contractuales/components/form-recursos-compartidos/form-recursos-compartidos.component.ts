import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-recursos-compartidos',
  templateUrl: './form-recursos-compartidos.component.html',
  styleUrls: ['./form-recursos-compartidos.component.scss']
})
export class FormRecursosCompartidosComponent implements OnInit {

  dataForm: any[] = [
    {
      nombre                  : 'Constructora Colpatria',
      tipoIdentificacion      : 'NIT',
      numeroIdentificacion    : '900.333.000-3',
      nombreRepresentanteLegal: 'Felipe Andrés Ruiz Fandiño',
      numeroInvitacion        : '00300',
      drp                     : 'DRP_9999',
      aportantes              : [
        {
          nombre        : 'FFIE',
          valorAportante: '76.000.000',
          uso           : [ 'Obra complementaria', 'Obra' ],
          valorUso      : [ '16.000.000', '60.000.000' ]
        },
        {
          nombre        : 'Gobernación del Valle Del Cauca',
          valorAportante: '70.000.000',
          uso           : [ 'Obra complementaria', 'Obra' ],
          valorUso      : [ '20.000.000', '50.000.000' ]
        },
        {
          nombre        : 'Fundación Pies Descalzos',
          valorAportante: '100.000.000',
          uso           : [ 'Obra' ],
          valorUso      : [ '100.000.000' ]
        }
      ]
    },
    {
      nombre                  : 'Consorcio AG',
      tipoIdentificacion      : 'NIT',
      numeroIdentificacion    : '900.555.777-6',
      nombreRepresentanteLegal: 'Daniel Alfredo Remolina Carvajal',
      numeroInvitacion        : '01234',
      drp                     : 'DRP_8888',
      aportantes              : [
        {
          nombre        : 'FFIE',
          valorAportante: '50.000.000',
          uso           : [ 'Obra complementaria', 'Obra' ],
          valorUso      : [ '10.000.000', '40.000.000' ]
        },
        {
          nombre        : 'Gobernación del Valle Del Cauca',
          valorAportante: '30.000.000',
          uso           : [ 'Obra complementaria', 'Obra' ],
          valorUso      : [ '10.000.000', '20.000.000' ]
        }
      ]
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
