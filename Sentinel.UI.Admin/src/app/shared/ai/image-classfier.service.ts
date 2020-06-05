import { Injectable } from '@angular/core';
import * as tf from '@tensorflow/tfjs';
import * as mobilenet from '@tensorflow-models/mobilenet';

@Injectable({
  providedIn: 'root'
})
export class ImageClassfierService {
  loading: boolean;


  linearModel: tf.Sequential;
  prediction: any;

  model: any; // tf.GraphModel; // any; // tf.Model;
  predictions: any;


  constructor() {


  }


  async trainNewModel() {
    // Define a model for linear regression.
    this.linearModel = tf.sequential();
    this.linearModel.add(tf.layers.dense({ units: 1, inputShape: [1] }));

    // Prepare the model for training: Specify the loss and the optimizer.
    this.linearModel.compile({ loss: 'meanSquaredError', optimizer: 'sgd' });


    // Training data, completely random stuff
    const xs = tf.tensor1d([3.2, 4.4, 5.5, 6.71, 6.98, 7.168, 9.779, 6.182, 7.59, 2.16, 7.042, 10.71, 5.313, 7.97, 5.654, 9.7, 3.11]);
    const ys = tf.tensor1d([1.6, 2.7, 2.9, 3.19, 1.684, 2.53, 3.366, 2.596, 2.53, 1.22, 2.87, 3.45, 1.65, 2.904, 2.42, 2.4, 1.31]);


    // Train
    await this.linearModel.fit(xs, ys);

    console.log('model trained!');
  }

  linearPrediction(val: number) {
    const output = this.linearModel.predict(tf.tensor2d([val], [1, 1])) as any;
    this.prediction = Array.from(output.dataSync())[0];
  }



  //// LOAD PRETRAINED KERAS MODEL ////

  async loadModel() {
    console.log('model is loading...');
    this.model = await tf.loadLayersModel('/assets/model.json');
    // this.model = await tf.loadGraphModel('/assets/model.json');
    console.log('model is loaded...');
  }

  async predict(imageData: ImageData) {

    const pred = tf.tidy(() => {

      // Convert the canvas pixels to
      let img = tf.browser.fromPixels(imageData, 1);
      // @ts-ignore
      img = img.reshape([1, 28, 28, 1]);
      img = tf.cast(img, 'float32');

      // Make and format the predications
      const output = this.model.predict(img) as any;

      // Save predictions on the component
      this.predictions = Array.from(output.dataSync());
      // return this.predictions;

      // const result = await this.model.executeAsync(img) as any;
      // const prediciton = Array.from(result.dataSync());

    });

  }






  // async loader() {
  //   this.loading = true;
  //   //  this.model = await mobilenet.load();
  //   this.loading = false;
  // }


  // async train(): Promise<any> {
  //   // Define a model for linear regression.
  //   this.linearModel = tf.sequential();
  //   this.linearModel.add(tf.layers.dense({ units: 1, inputShape: [1] }));

  //   // Prepare the model for training: Specify the loss and the optimizer.
  //   this.linearModel.compile({ loss: 'meanSquaredError', optimizer: 'sgd' });

  //   // Training data, completely random stuff
  //   const xs = tf.tensor1d([3.2, 4.4, 5.5]);
  //   const ys = tf.tensor1d([1.6, 2.7, 3.5]);

  //   // Train
  //   await this.linearModel.fit(xs, ys);

  //   console.log('model trained!');
  // }

  // predict(val: number) {
  //   const output = this.linearModel.predict(tf.tensor2d([val], [1, 1])) as any;
  //   this.prediction = Array.from(output.dataSync())[0];
  //   console.log(this.prediction);
  // }

}
